using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Repositories;
using InventoryAPI.UnitOfWork;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Commands.UpdateProveedor;

public class UpdateProveedorCommandHandler : IRequestHandler<UpdateProveedorCommand, ProveedorResponseDto>
{
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProveedorCommandHandler(IProveedorRepository proveedorRepository, IUnitOfWork unitOfWork)
    {
        _proveedorRepository = proveedorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProveedorResponseDto> Handle(UpdateProveedorCommand request, CancellationToken cancellationToken)
    {
        var proveedor = await _proveedorRepository.GetById(request.Id);

        if (proveedor == null)
            throw new KeyNotFoundException($"Proveedor con ID {request.Id} no encontrado");

        // Actualización parcial
        if (request.Nombre != null)
            proveedor.Nombre = request.Nombre;

        if (request.Email != null)
            proveedor.Email = request.Email;

        if (request.Telefono != null)
            proveedor.Telefono = request.Telefono;

        var proveedorActualizado = await _proveedorRepository.Update(proveedor);
        await _unitOfWork.SaveChangesAsync();

        var response = new ProveedorResponseDto
        {
            Id = proveedorActualizado!.Id,
            Nombre = proveedorActualizado.Nombre,
            Email = proveedorActualizado.Email,
            Telefono = proveedorActualizado.Telefono
        };

        return response;
    }
}