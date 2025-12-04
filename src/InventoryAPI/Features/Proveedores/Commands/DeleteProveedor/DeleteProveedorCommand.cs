using MediatR;

namespace InventoryAPI.Features.Proveedores.Commands.DeleteProveedor;

public record DeleteProveedorCommand(int Id) : IRequest<bool>;