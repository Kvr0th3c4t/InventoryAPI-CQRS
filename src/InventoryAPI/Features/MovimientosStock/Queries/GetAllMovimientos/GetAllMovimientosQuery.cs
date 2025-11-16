using InventoryAPI.Dtos.MovimientoStockDtos;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Queries.GetAllMovimientos;

public record GetAllMovimientosQuery() : IRequest<IEnumerable<MovimientoStockResponseDto>>;