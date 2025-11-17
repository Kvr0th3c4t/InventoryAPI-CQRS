using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Queries.GetAllMovimientos;

public class GetAllMovimientosQueryHandler : IRequestHandler<GetAllMovimientosQuery, IEnumerable<MovimientoStockResponseDto>>
{
    private readonly IMovimientoStockRepository _movimientosRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IProveedorRepository _proveedorRepository;

    public GetAllMovimientosQueryHandler(
        IMovimientoStockRepository movimientoStockRepository,
        IProductoRepository productoRepository,
        IProveedorRepository proveedorRepository)
    {
        _movimientosRepository = movimientoStockRepository;
        _productoRepository = productoRepository;
        _proveedorRepository = proveedorRepository;
    }

    public Task<IEnumerable<MovimientoStockResponseDto>> Handle(GetAllMovimientosQuery request, CancellationToken cancellationToken)
    {
        var movimientos = _movimientosRepository.GetAll();

        var result = movimientos
            .Select(movimiento =>
            {
                var producto = _productoRepository.GetById(movimiento.ProductoId);

                var proveedor = movimiento.ProveedorId.HasValue
                    ? _proveedorRepository.GetById(movimiento.ProveedorId.Value)
                    : null;

                return new MovimientoStockResponseDto
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
            });

        return Task.FromResult(result);
    }
}