using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Enums;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Commands.CreateMovimiento;

public record CreateMovimientoStockCommand(
    int ProductoId,
    int? ProveedorId,
    TipoMovimiento Tipo,
    int Cantidad,
    string? Razon
) : IRequest<MovimientoStockResponseDto>;