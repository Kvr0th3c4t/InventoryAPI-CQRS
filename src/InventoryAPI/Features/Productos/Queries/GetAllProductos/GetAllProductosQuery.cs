using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.ProductoDtos;
using MediatR;

namespace InventoryAPI.Features.Productos.Queries.GetAllProductos;

public record GetAllProductosQuery(int Page = 1, int PageSize = 10)
    : IRequest<PagedResponse<ProductoResponseDto>>;