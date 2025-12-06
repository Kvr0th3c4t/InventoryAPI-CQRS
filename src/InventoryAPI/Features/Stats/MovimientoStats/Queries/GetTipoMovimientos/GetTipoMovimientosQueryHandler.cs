using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetTipoMovimientos;

public class GetTipoMovimientosQueryHandler : IRequestHandler<GetTipoMovimientosQuery, PagedResponse<TipoMovimientoDto>>
{
    private readonly IMovimientoStockRepository _movimientoStockRepository;
    public GetTipoMovimientosQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientoStockRepository = movimientoStockRepository;
    }

    public async Task<PagedResponse<TipoMovimientoDto>> Handle(GetTipoMovimientosQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;
        return await _movimientoStockRepository.GetTipoMovimientosAsync(pageNumber, pageSize);
    }
}