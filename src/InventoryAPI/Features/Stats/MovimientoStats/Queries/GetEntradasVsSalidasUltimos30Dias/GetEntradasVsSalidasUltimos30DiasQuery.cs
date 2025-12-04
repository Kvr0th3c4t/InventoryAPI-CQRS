using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetEntradasVsSalidasUltimos30Dias;

public record GetEntradasVsSalidasUltimos30DiasQuery : IRequest<EntradasVsSalidasDto>;