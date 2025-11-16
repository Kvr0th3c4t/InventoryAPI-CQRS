using InventoryAPI.Dtos.ProveedorDtos;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Commands.UpdateProveedor;

public record UpdateProveedorCommand(
    int Id,
    string? Nombre,
    string? Email,
    string? Telefono
) : IRequest<ProveedorResponseDto>;