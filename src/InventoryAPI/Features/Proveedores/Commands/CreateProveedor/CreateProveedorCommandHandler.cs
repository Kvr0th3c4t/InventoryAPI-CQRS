using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using InventoryAPI.UnitOfWork;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Commands.CreateProveedor;

public class CreateProveedorCommandHandler : IRequestHandler<CreateProveedorCommand, ProveedorResponseDto>
{
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProveedorCommandHandler(IProveedorRepository proveedorRepository, IUnitOfWork unitOfWork)
    {
        _proveedorRepository = proveedorRepository;
        _unitOfWork = unitOfWork;
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
        await _unitOfWork.SaveChangesAsync();


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