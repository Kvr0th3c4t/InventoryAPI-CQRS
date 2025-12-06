using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Enums;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Queries.GetAllMovimientos;

public record GetAllMovimientosQuery(
    DateTimeOffset? FechaDesde = null,
    DateTimeOffset? FechaHasta = null,
    TipoMovimiento? Tipo = null,
    int? ProductoId = null,
    string? OrderBy = null,
    string? Order = null,
    int Page = 1,
    int PageSize = 10
) : IRequest<PagedResponse<MovimientoStockResponseDto>>;
