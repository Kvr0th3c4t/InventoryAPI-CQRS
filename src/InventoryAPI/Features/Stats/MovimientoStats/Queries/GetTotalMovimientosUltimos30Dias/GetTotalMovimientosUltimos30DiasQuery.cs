using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetTotalMovimientosUltimos30Dias;

public record GetTotalMovimientosUltimos30DiasQuery : IRequest<int>;