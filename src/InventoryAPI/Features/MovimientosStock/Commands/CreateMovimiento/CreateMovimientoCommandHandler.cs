using System.ComponentModel;
using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Events;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using InventoryAPI.Events;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Commands.CreateMovimiento;

public class CreateMovimientoStockCommandHandler : IRequestHandler<CreateMovimientoStockCommand, MovimientoStockResponseDto>
{
    private readonly IMovimientoStockRepository _movimientosRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IEventPublisher _eventPublisher;

    public CreateMovimientoStockCommandHandler(IMovimientoStockRepository movimientoStockRepository, IProductoRepository productoRepository, IProveedorRepository proveedorRepository, IEventPublisher eventPublisher)
    {
        _movimientosRepository = movimientoStockRepository;
        _productoRepository = productoRepository;
        _proveedorRepository = proveedorRepository;
        _eventPublisher = eventPublisher;
    }

    public Task<MovimientoStockResponseDto> Handle(CreateMovimientoStockCommand request, CancellationToken cancellationToken)
    {
        var productoExiste = _productoRepository.GetById(request.ProductoId);

        if (productoExiste == null)
        {
            throw new InvalidOperationException("No existe el producto");
        }

        if (request.Tipo == Enums.TipoMovimiento.Entrada)
        {
            if (!request.ProveedorId.HasValue)
                throw new ArgumentException("Para movimientos de entrada es obligatorio el ProveedorId");

            var proveedorExiste = _proveedorRepository.GetById(request.ProveedorId.Value);

            if (proveedorExiste == null)
                throw new InvalidOperationException("El proveedor no existe");
        }

        if (request.Cantidad <= 0)
        {
            throw new ArgumentException("La cantidad debe ser mayor a 0");
        }

        if (request.Tipo == Enums.TipoMovimiento.Salida || request.Tipo == Enums.TipoMovimiento.AjusteNegativo)
        {
            if (productoExiste.StockActual < request.Cantidad)
            {
                throw new InvalidOperationException(
                    $"Stock insuficiente. Stock actual: {productoExiste.StockActual}, " +
                    $"cantidad solicitada: {request.Cantidad}");
            }
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

        _productoRepository.Update(productoExiste);

        if (productoExiste.StockActual < productoExiste.StockMinimo)
        {
            var evento = new StockBajoEvent
            {
                ProductoId = productoExiste.Id,
                ProductoNombre = productoExiste.Nombre,
                StockActual = productoExiste.StockActual,
                StockMinimo = productoExiste.StockMinimo,
                FechaEvento = DateTime.Now
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
            FechaMovimiento = DateTime.Now
        };

        var movimientoCreado = _movimientosRepository.Add(movimiento);

        var producto = _productoRepository.GetById(movimientoCreado.ProductoId);
        var proveedor = movimientoCreado.ProveedorId.HasValue
            ? _proveedorRepository.GetById(movimientoCreado.ProveedorId.Value)
            : null;

        var response = new MovimientoStockResponseDto
        {
            Id = movimientoCreado.Id,
            ProductoId = movimientoCreado.ProductoId,
            ProductoNombre = producto?.Nombre,
            ProveedorId = movimientoCreado.ProveedorId,
            ProveedorNombre = proveedor?.Nombre,
            Tipo = movimientoCreado.Tipo,
            Cantidad = movimientoCreado.Cantidad,
            Fecha = movimientoCreado.FechaMovimiento,
            Razon = movimientoCreado.Razon
        };

        return Task.FromResult(response);
    }
}