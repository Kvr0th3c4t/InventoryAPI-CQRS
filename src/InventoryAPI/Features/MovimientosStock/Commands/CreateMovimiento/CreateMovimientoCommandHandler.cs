using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Events;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Commands.CreateMovimiento;

public class CreateMovimientoStockCommandHandler : IRequestHandler<CreateMovimientoStockCommand, MovimientoStockResponseDto>
{
    private readonly IMovimientoStockRepository _movimientosRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IEventPublisher _eventPublisher;

    public CreateMovimientoStockCommandHandler(
        IMovimientoStockRepository movimientosRepository,
        IProductoRepository productoRepository,
        IProveedorRepository proveedorRepository,
        IEventPublisher eventPublisher)
    {
        _movimientosRepository = movimientosRepository;
        _productoRepository = productoRepository;
        _proveedorRepository = proveedorRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<MovimientoStockResponseDto> Handle(CreateMovimientoStockCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar producto existe
        var productoExiste = await _productoRepository.GetById(request.ProductoId);
        if (productoExiste == null)
            throw new InvalidOperationException("No existe el producto");

        // 2. Si es Entrada → Validar ProveedorId obligatorio
        if (request.Tipo == Enums.TipoMovimiento.Entrada)
        {
            if (!request.ProveedorId.HasValue)
                throw new ArgumentException("Para movimientos de entrada es obligatorio el ProveedorId");

            var proveedorExiste = await _proveedorRepository.GetById(request.ProveedorId.Value);
            if (proveedorExiste == null)
                throw new InvalidOperationException("El proveedor no existe");
        }

        // 3. Validar cantidad > 0
        if (request.Cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0");

        // 4. Si es Salida/AjusteNegativo → Validar stock suficiente
        if (request.Tipo == Enums.TipoMovimiento.Salida || request.Tipo == Enums.TipoMovimiento.AjusteNegativo)
        {
            if (productoExiste.StockActual < request.Cantidad)
                throw new InvalidOperationException(
                    $"Stock insuficiente. Stock actual: {productoExiste.StockActual}, cantidad solicitada: {request.Cantidad}");
        }

        // 5. Actualizar stock según tipo de movimiento
        switch (request.Tipo)
        {
            case Enums.TipoMovimiento.Entrada:
            case Enums.TipoMovimiento.AjustePositivo:
                productoExiste.StockActual += request.Cantidad;
                break;

            case Enums.TipoMovimiento.Salida:
            case Enums.TipoMovimiento.AjusteNegativo:
                productoExiste.StockActual -= request.Cantidad;
                break;
        }

        await _productoRepository.Update(productoExiste);

        // 6. Disparar StockBajoEvent si corresponde
        if (productoExiste.StockActual < productoExiste.StockMinimo)
        {
            var evento = new StockBajoEvent
            {
                ProductoId = productoExiste.Id,
                ProductoNombre = productoExiste.Nombre,
                StockActual = productoExiste.StockActual,
                StockMinimo = productoExiste.StockMinimo,
                FechaEvento = DateTimeOffset.UtcNow
            };
            _eventPublisher.Publish(evento);
        }

        // 7. Crear el movimiento
        var movimiento = new MovimientoStock
        {
            ProductoId = request.ProductoId,
            ProveedorId = request.ProveedorId,
            Tipo = request.Tipo,
            Cantidad = request.Cantidad,
            Razon = request.Razon,
            FechaMovimiento = DateTimeOffset.UtcNow
        };

        var movimientoCreado = await _movimientosRepository.Add(movimiento);

        // 8. Devolver DTO con nombres de producto y proveedor
        var producto = await _productoRepository.GetById(movimientoCreado.ProductoId);
        var proveedor = movimientoCreado.ProveedorId.HasValue
            ? await _proveedorRepository.GetById(movimientoCreado.ProveedorId.Value)
            : null;

        var response = new MovimientoStockResponseDto
        {
            Id = movimientoCreado.Id,
            ProductoId = movimientoCreado.ProductoId,
            ProductoNombre = producto?.Nombre ?? string.Empty,
            ProveedorId = movimientoCreado.ProveedorId,
            ProveedorNombre = proveedor?.Nombre,
            Tipo = movimientoCreado.Tipo,
            Cantidad = movimientoCreado.Cantidad,
            Fecha = movimientoCreado.FechaMovimiento,
            Razon = movimientoCreado.Razon
        };

        return response;
    }
}