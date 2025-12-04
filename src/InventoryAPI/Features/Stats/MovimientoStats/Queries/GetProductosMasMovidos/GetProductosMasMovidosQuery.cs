using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetProductosMasMovidos;

public record GetProductosMasMovidosQuery : IRequest<IEnumerable<ProductoMasMovidoDto>>;