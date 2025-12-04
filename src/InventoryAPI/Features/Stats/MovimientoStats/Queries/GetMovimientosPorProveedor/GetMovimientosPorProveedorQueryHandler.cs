using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetMovimientosPorProveedor;

public class GetMovimientosPorProveedorQueryHandler : IRequestHandler<GetMovimientosPorProveedorQuery, IEnumerable<MovimientoPorProveedorDto>>
{
    private readonly IMovimientoStockRepository _movimientoStockRepository;
    public GetMovimientosPorProveedorQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientoStockRepository = movimientoStockRepository;
    }

    public async Task<IEnumerable<MovimientoPorProveedorDto>> Handle(GetMovimientosPorProveedorQuery request, CancellationToken cancellationToken)
    {
        return await _movimientoStockRepository.GetMovimientosPorProveedorAsync();
    }
}