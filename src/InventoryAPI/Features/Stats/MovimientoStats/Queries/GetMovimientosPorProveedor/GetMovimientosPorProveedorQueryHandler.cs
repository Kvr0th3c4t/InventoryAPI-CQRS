using Azure;
using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetMovimientosPorProveedor;

public class GetMovimientosPorProveedorQueryHandler : IRequestHandler<GetMovimientosPorProveedorQuery, PagedResponse<MovimientoPorProveedorDto>>
{
    private readonly IMovimientoStockRepository _movimientoStockRepository;
    public GetMovimientosPorProveedorQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientoStockRepository = movimientoStockRepository;
    }

    public async Task<PagedResponse<MovimientoPorProveedorDto>> Handle(GetMovimientosPorProveedorQuery request, CancellationToken cancellationToken)

    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;

        return await _movimientoStockRepository.GetMovimientosPorProveedorAsync(pageNumber, pageSize);
    }
}