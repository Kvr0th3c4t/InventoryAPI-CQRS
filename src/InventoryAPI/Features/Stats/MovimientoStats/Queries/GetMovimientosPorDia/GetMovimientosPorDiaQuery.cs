using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetMovimientosPorDia;

public record GetMovimientosPorDiaQuery : IRequest<IEnumerable<MovimientoPorDiaDto>>;