using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetTipoMovimientos;

public class GetTipoMovimientosQueryHandler : IRequestHandler<GetTipoMovimientosQuery, IEnumerable<TipoMovimientoDto>>
{
    private readonly IMovimientoStockRepository _movimientoStockRepository;
    public GetTipoMovimientosQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientoStockRepository = movimientoStockRepository;
    }

    public async Task<IEnumerable<TipoMovimientoDto>> Handle(GetTipoMovimientosQuery request, CancellationToken cancellationToken)
    {
        return await _movimientoStockRepository.GetTipoMovimientosAsync();
    }
}