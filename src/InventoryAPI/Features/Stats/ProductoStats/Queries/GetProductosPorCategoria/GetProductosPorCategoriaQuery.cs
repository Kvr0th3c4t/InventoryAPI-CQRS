using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosPorCategoria;

public record GetProductosPorCategoriaQuery(int PageNumber = 1, int PageSize = 1) : IRequest<PagedResponse<DistribucionCategoriaDto>>;