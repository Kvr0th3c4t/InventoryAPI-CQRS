using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosPorProveedor;

public class GetProductosPorProveedorQueryHandler : IRequestHandler<GetProductosPorProveedorQuery, PagedResponse<DistribucionProveedorDto>>
{
    private readonly IProductoRepository _productoRepository;

    public GetProductosPorProveedorQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<PagedResponse<DistribucionProveedorDto>> Handle(GetProductosPorProveedorQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;

        return await _productoRepository.GetProductosPorProveedorAsync(pageNumber, pageSize);
    }

}
