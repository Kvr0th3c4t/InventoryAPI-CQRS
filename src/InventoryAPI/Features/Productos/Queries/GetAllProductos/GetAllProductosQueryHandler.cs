using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Repositories;
using InventoryAPI.Dtos.Pagination;
using MediatR;

namespace InventoryAPI.Features.Productos.Queries.GetAllProductos;

public class GetAllProductosQueryHandler
    : IRequestHandler<GetAllProductosQuery, PagedResponse<ProductoResponseDto>>
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
        // Validar paginación
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;

        // Validar ordenamiento
        var validOrderByFields = new[] { "nombre", "precio", "stock", "categoria", "proveedor" };
        var orderBy = !string.IsNullOrWhiteSpace(request.OrderBy) &&
                      validOrderByFields.Contains(request.OrderBy.ToLower())
            ? request.OrderBy.ToLower()
            : "nombre";

        var order = request.Order?.ToLower() == "desc" ? "desc" : "asc";

        // Llamar al repository con todos los filtros
        return await _productoRepository.GetAllPaginated(
            search: request.Search,
            categoriaId: request.CategoriaId,
            proveedorId: request.ProveedorId,
            precioMin: request.PrecioMin,
            precioMax: request.PrecioMax,
            stockBajo: request.StockBajo ?? false,
            orderBy: orderBy,
            order: order,
            pageNumber: pageNumber,
            pageSize: pageSize
        );
    }
}
