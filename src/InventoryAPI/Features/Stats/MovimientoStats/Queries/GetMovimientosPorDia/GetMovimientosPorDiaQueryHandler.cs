using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetMovimientosPorDia;

public class GetMovimientosPorDiaQueryHandler : IRequestHandler<GetMovimientosPorDiaQuery, PagedResponse<MovimientoPorDiaDto>>
{
    private readonly IMovimientoStockRepository _movimientoStockRepository;
    public GetMovimientosPorDiaQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientoStockRepository = movimientoStockRepository;
    }

    public async Task<PagedResponse<MovimientoPorDiaDto>> Handle(GetMovimientosPorDiaQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;

        return await _movimientoStockRepository.GetMovimientosPorDiaAsync(pageNumber, pageSize);
    }
}