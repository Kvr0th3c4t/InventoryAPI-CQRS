using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.StatsDtos.ProveedoresStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.ProveedorStats.Queries.GetValorInventarioPorProveedor;

public record GetValorInventarioPorProveedorQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagedResponse<DistribucionValorProveedorDto>>;