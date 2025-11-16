using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Commands.UpdateProveedor;

public class UpdateProveedorCommandHandler : IRequestHandler<UpdateProveedorCommand, ProveedorResponseDto>
{
    private readonly IProveedorRepository _proveedorRepository;

    public UpdateProveedorCommandHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public Task<ProveedorResponseDto> Handle(UpdateProveedorCommand request, CancellationToken cancellationToken)
    {
        var proveedor = _proveedorRepository.GetById(request.Id);

        if (proveedor == null)
            throw new ArgumentException("El proveedor no existe");

        if (request.Nombre != null) proveedor.Nombre = request.Nombre;
        if (request.Email != null) proveedor.Email = request.Email;
        if (request.Telefono != null) proveedor.Telefono = request.Telefono;

        var proveedorModificado = _proveedorRepository.Update(proveedor);

        var response = new ProveedorResponseDto
        {
            Id = proveedorModificado.Id,
            Nombre = proveedorModificado.Nombre,
            Email = proveedorModificado.Email,
            Telefono = proveedorModificado.Telefono
        };

        return Task.FromResult(response);
    }
}