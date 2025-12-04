using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.ProveedorStats.Queries.GetProductosPorProveedor;

public record GetProductosPorProveedorQuery : IRequest<IEnumerable<DistribucionProveedorDto>>;