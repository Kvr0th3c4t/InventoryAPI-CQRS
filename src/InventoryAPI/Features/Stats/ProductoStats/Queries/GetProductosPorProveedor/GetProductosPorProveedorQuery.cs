using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosPorProveedor;

public record GetProductosPorProveedorQuery() : IRequest<IEnumerable<DistribucionProveedorDto>>;