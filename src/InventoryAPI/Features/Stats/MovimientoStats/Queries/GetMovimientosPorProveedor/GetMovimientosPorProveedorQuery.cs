using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetMovimientosPorProveedor;

public record GetMovimientosPorProveedorQuery : IRequest<IEnumerable<MovimientoPorProveedorDto>>;