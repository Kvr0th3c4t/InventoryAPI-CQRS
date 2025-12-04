using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetMovimientosPorDia;

public class GetMovimientosPorDiaQueryHandler : IRequestHandler<GetMovimientosPorDiaQuery, IEnumerable<MovimientoPorDiaDto>>
{
    private readonly IMovimientoStockRepository _movimientoStockRepository;
    public GetMovimientosPorDiaQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientoStockRepository = movimientoStockRepository;
    }

    public async Task<IEnumerable<MovimientoPorDiaDto>> Handle(GetMovimientosPorDiaQuery request, CancellationToken cancellationToken)
    {
        return await _movimientoStockRepository.GetMovimientosPorDiaAsync();
    }
}