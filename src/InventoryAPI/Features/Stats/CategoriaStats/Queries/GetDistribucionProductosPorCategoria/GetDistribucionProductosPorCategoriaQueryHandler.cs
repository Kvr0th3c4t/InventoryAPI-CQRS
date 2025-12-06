using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.CategoriaStats.Queries.GetDistribucionProductosPorCategoria;

public class GetDistribucionProductosPorCategoriaQueryHandler : IRequestHandler<GetDistribucionProductosPorCategoriaQuery, PagedResponse<DistribucionCategoriaDto>>
{
    private readonly ICategoriaRepository _categoriaRepository;
    public GetDistribucionProductosPorCategoriaQueryHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<PagedResponse<DistribucionCategoriaDto>> Handle(GetDistribucionProductosPorCategoriaQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;

        return await _categoriaRepository.GetDistribucionProductosPorCategoriaAsync(pageNumber, pageSize);
    }
}
