using InventoryAPI.Dtos.StatsDtos.ProveedoresStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.ProveedorStats.Queries.GetValorInventarioPorProveedor;

public record GetValorInventarioPorProveedorQuery : IRequest<IEnumerable<DistribucionValorProveedorDto>>;