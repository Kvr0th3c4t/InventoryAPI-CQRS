using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.CategoriaStats.Queries.GetDistribucionProductosPorCategoria;

public record GetDistribucionProductosPorCategoriaQuery(int PageNumber = 1, int PageSize = 1) : IRequest<PagedResponse<DistribucionCategoriaDto>>
{
}