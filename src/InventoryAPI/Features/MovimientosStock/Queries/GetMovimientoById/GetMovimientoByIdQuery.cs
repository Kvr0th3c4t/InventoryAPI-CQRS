using InventoryAPI.Dtos.MovimientoStockDtos;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Queries.GetMovimientoById;

public record GetMovimientoByIdQuery(int Id) : IRequest<MovimientoStockResponseDto>;