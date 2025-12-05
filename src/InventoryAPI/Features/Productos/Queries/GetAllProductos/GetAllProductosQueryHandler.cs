using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Repositories;
using InventoryAPI.Dtos.Pagination;
using MediatR;

namespace InventoryAPI.Features.Productos.Queries.GetAllProductos;

public class GetAllProductosQueryHandler : IRequestHandler<GetAllProductosQuery, PagedResponse<ProductoResponseDto>>
{
    private readonly IProductoRepository _productoRepository;

    public GetAllProductosQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<PagedResponse<ProductoResponseDto>> Handle(
       GetAllProductosQuery request,
       CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;

        return await _productoRepository.GetAllPaginated(page, pageSize);
    }
}
