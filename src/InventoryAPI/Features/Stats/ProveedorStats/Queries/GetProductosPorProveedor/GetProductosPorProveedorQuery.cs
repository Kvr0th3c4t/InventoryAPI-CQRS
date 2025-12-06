using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.ProveedorStats.Queries.GetProductosPorProveedor;

public record GetProductosPorProveedorQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagedResponse<DistribucionProveedorDto>>;