using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Queries.GetMovimientoById;

public class GetMovimientoByIdQueryHandler : IRequestHandler<GetMovimientoByIdQuery, MovimientoStockResponseDto>
{
    private readonly IMovimientoStockRepository _movimientoRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IProveedorRepository _proveedorRepository;

    public GetMovimientoByIdQueryHandler(
        IMovimientoStockRepository movimientoStockRepository,
        IProductoRepository productoRepository,
        IProveedorRepository proveedorRepository)
    {
        _movimientoRepository = movimientoStockRepository;
        _productoRepository = productoRepository;
        _proveedorRepository = proveedorRepository;
    }

    public Task<MovimientoStockResponseDto> Handle(GetMovimientoByIdQuery request, CancellationToken cancellationToken)
    {
        var movimiento = _movimientoRepository.GetById(request.Id);

        if (movimiento == null)
        {
            throw new KeyNotFoundException($"El movimiento con Id {request.Id} no existe");
        }

        var producto = _productoRepository.GetById(movimiento.ProductoId);

        var proveedor = movimiento.ProveedorId.HasValue
            ? _proveedorRepository.GetById(movimiento.ProveedorId.Value)
            : null;

        var result = new MovimientoStockResponseDto
        {
            Id = movimiento.Id,
            ProductoId = movimiento.ProductoId,
            ProductoNombre = producto?.Nombre,
            ProveedorId = movimiento.ProveedorId,
            ProveedorNombre = proveedor?.Nombre,
            Tipo = movimiento.Tipo,
            Cantidad = movimiento.Cantidad,
            Razon = movimiento.Razon,
            Fecha = movimiento.FechaMovimiento
        };

        return Task.FromResult(result);
    }
}