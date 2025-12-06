using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetMovimientosPorDia;

public record GetMovimientosPorDiaQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagedResponse<MovimientoPorDiaDto>>;