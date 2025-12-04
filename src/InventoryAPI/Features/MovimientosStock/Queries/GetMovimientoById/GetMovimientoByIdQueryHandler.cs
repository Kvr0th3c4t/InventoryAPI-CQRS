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
        IMovimientoStockRepository movimientoRepository,
        IProductoRepository productoRepository,
        IProveedorRepository proveedorRepository)
    {
        _movimientoRepository = movimientoRepository;
        _productoRepository = productoRepository;
        _proveedorRepository = proveedorRepository;
    }

    public async Task<MovimientoStockResponseDto> Handle(GetMovimientoByIdQuery request, CancellationToken cancellationToken)
    {
        var movimiento = await _movimientoRepository.GetById(request.Id);

        if (movimiento == null)
            throw new KeyNotFoundException($"El movimiento con Id {request.Id} no existe");

        // Obtener el producto
        var producto = await _productoRepository.GetById(movimiento.ProductoId);

        // Obtener el proveedor (solo si existe)
        var proveedor = movimiento.ProveedorId.HasValue
            ? await _proveedorRepository.GetById(movimiento.ProveedorId.Value)
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

        return result;
    }
}