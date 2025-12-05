using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.Pagination;
using MediatR;

namespace InventoryAPI.Features.Categorias.Queries.GetAllCategorias;

public record GetAllCategoriasQuery(int Page = 1, int PageSize = 10)
    : IRequest<PagedResponse<CategoriaResponseDto>>;