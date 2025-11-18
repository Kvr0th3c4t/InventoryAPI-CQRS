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
        IMovimientoStockRepository movimientosRepository,
        IProductoRepository productoRepository,
        IProveedorRepository proveedorRepository)
    {
        _movimientosRepository = movimientosRepository;
        _productoRepository = productoRepository;
        _proveedorRepository = proveedorRepository;
    }

    public async Task<IEnumerable<MovimientoStockResponseDto>> Handle(GetAllMovimientosQuery request, CancellationToken cancellationToken)
    {
        var movimientos = await _movimientosRepository.GetAll();

        var result = new List<MovimientoStockResponseDto>();

        foreach (var movimiento in movimientos)
        {
            // Obtener el producto
            var producto = await _productoRepository.GetById(movimiento.ProductoId);

            // Obtener el proveedor (solo si existe)
            var proveedor = movimiento.ProveedorId.HasValue
                ? await _proveedorRepository.GetById(movimiento.ProveedorId.Value)
                : null;

            result.Add(new MovimientoStockResponseDto
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
            });
        }

        return result;
    }
}