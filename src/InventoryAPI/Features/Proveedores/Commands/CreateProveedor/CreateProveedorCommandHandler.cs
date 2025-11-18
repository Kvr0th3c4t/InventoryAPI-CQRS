using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Commands.CreateProveedor;

public class CreateProveedorCommandHandler : IRequestHandler<CreateProveedorCommand, ProveedorResponseDto>
{
    private readonly IProveedorRepository _proveedorRepository;

    public CreateProveedorCommandHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<ProveedorResponseDto> Handle(CreateProveedorCommand request, CancellationToken cancellationToken)
    {
        var proveedor = new Proveedor
        {
            Nombre = request.Nombre,
            Email = request.Email,
            Telefono = request.Telefono
        };

        var proveedorCreado = await _proveedorRepository.Add(proveedor);

        var response = new ProveedorResponseDto
        {
            Id = proveedorCreado.Id,
            Nombre = proveedorCreado.Nombre,
            Email = proveedorCreado.Email,
            Telefono = proveedorCreado.Telefono
        };

        return response;
    }
}