using InventoryAPI.Dtos.ProveedorDtos;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Commands.CreateProveedor;

public record CreateProveedorCommand(
    string Nombre,
    string? Email,
    string? Telefono

) : IRequest<ProveedorResponseDto>;