using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Queries.GetMovimientoById;

public class GetMovimientoByIdQueryHandler : IRequestHandler<GetMovimientoByIdQuery, MovimientoStockResponseDto>
{
    private readonly IMovimientoStockRepository _movimientoRepository;

    public GetMovimientoByIdQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientoRepository = movimientoStockRepository;
    }

    public Task<MovimientoStockResponseDto> Handle(GetMovimientoByIdQuery request, CancellationToken cancellationToken)
    {
        var movimiento = _movimientoRepository.GetById(request.Id);

        if (movimiento == null)
        {
            throw new KeyNotFoundException($"El movimiento con Id {request.Id} no existe");
        }

        var result = new MovimientoStockResponseDto
        {

            Id = movimiento.Id,
            ProductoId = movimiento.ProductoId,
            ProductoNombre = movimiento.Producto?.Nombre,
            ProveedorId = movimiento.ProveedorId,
            ProveedorNombre = movimiento.Proveedor?.Nombre,
            Tipo = movimiento.Tipo,
            Cantidad = movimiento.Cantidad,
            Razon = movimiento.Razon,
            Fecha = movimiento.FechaMovimiento
        };

        return Task.FromResult(result);
    }
}