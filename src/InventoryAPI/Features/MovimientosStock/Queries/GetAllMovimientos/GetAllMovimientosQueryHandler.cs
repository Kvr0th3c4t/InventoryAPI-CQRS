using System.ComponentModel;
using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Queries.GetAllMovimientos;

public class GetAllMovimientosQueryHandler : IRequestHandler<GetAllMovimientosQuery, IEnumerable<MovimientoStockResponseDto>>
{
    private readonly IMovimientoStockRepository _movimientosRepository;

    public GetAllMovimientosQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientosRepository = movimientoStockRepository;
    }

    public Task<IEnumerable<MovimientoStockResponseDto>> Handle(GetAllMovimientosQuery request, CancellationToken cancellationToken)
    {
        var movimientos = _movimientosRepository.GetAll();

        var result = movimientos
            .Select(movimiento =>
            {
                return new MovimientoStockResponseDto
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
            });

        return Task.FromResult(result);
    }
}