using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.ProductoDtos;
using MediatR;

namespace InventoryAPI.Features.Productos.Queries.GetAllProductos;

public record GetAllProductosQuery(

    string? Search = null,
    int? CategoriaId = null,
    int? ProveedorId = null,
    decimal? PrecioMin = null,
    decimal? PrecioMax = null,
    bool? StockBajo = null,
    string? OrderBy = null,
    string? Order = null,
    int PageNumber = 1,
    int PageSize = 10
    )
    : IRequest<PagedResponse<ProductoResponseDto>>;