using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetTipoMovimientos;

public record GetTipoMovimientosQuery : IRequest<IEnumerable<TipoMovimientoDto>>;