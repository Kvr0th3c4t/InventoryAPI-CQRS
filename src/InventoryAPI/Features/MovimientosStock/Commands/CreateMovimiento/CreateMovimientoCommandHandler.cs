using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Events;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using InventoryAPI.UnitOfWork;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Commands.CreateMovimiento;

public class CreateMovimientoStockCommandHandler : IRequestHandler<CreateMovimientoStockCommand, MovimientoStockResponseDto>
{
    private readonly IMovimientoStockRepository _movimientosRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMovimientoStockCommandHandler(
        IMovimientoStockRepository movimientosRepository,
        IProductoRepository productoRepository,
        IProveedorRepository proveedorRepository,
        IEventPublisher eventPublisher,
        IUnitOfWork unitOfWork)
    {
        _movimientosRepository = movimientosRepository;
        _productoRepository = productoRepository;
        _proveedorRepository = proveedorRepository;
        _eventPublisher = eventPublisher;
        _unitOfWork = unitOfWork;
    }

    public async Task<MovimientoStockResponseDto> Handle(CreateMovimientoStockCommand request, CancellationToken cancellationToken)
    {

        var productoExiste = await _productoRepository.GetById(request.ProductoId);
        if (productoExiste == null)
            throw new InvalidOperationException("No existe el producto");

        if (request.ProveedorId.HasValue)
        {
            var proveedorExiste = await _proveedorRepository.GetById(request.ProveedorId.Value);
            if (proveedorExiste == null)
                throw new InvalidOperationException("El proveedor no existe");
        }

        if (request.Tipo == Enums.TipoMovimiento.Salida || request.Tipo == Enums.TipoMovimiento.AjusteNegativo)
        {
            if (productoExiste.StockActual < request.Cantidad)
                throw new InvalidOperationException(
                    $"Stock insuficiente. Stock actual: {productoExiste.StockActual}, cantidad solicitada: {request.Cantidad}");
        }

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

        if (productoExiste.StockActual < productoExiste.StockMinimo)
        {
            var evento = new StockBajoEvent
            {
                ProductoId = productoExiste.Id,
                ProductoNombre = productoExiste.Nombre,
                StockActual = productoExiste.StockActual,
                StockMinimo = productoExiste.StockMinimo,
            };
            _eventPublisher.Publish(evento);
        }

        var movimiento = new MovimientoStock
        {
            ProductoId = request.ProductoId,
            ProveedorId = request.ProveedorId,
            Tipo = request.Tipo,
            Cantidad = request.Cantidad,
            Razon = request.Razon,
        };

        var movimientoCreado = await _movimientosRepository.Add(movimiento);

        await _unitOfWork.SaveChangesAsync();

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