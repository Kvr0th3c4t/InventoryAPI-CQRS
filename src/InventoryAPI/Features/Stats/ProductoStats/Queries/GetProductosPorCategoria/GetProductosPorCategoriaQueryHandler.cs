using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosPorCategoria;

public class GetProductosPorCategoriaQueryHandler : IRequestHandler<GetProductosPorCategoriaQuery, PagedResponse<DistribucionCategoriaDto>>
{
    private readonly IProductoRepository _productoRepository;

    public GetProductosPorCategoriaQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<PagedResponse<DistribucionCategoriaDto>> Handle(GetProductosPorCategoriaQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;

        return await _productoRepository.GetProductosPorCategoriaAsync(pageNumber, pageSize);
    }

}
