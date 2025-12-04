using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Queries.GetProveedorById;

public class GetProveedorByIdQueryHandler : IRequestHandler<GetProveedorByIdQuery, ProveedorResponseDto>
{
    private readonly IProveedorRepository _proveedorRepository;

    public GetProveedorByIdQueryHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<ProveedorResponseDto> Handle(GetProveedorByIdQuery request, CancellationToken cancellationToken)
    {
        var proveedor = await _proveedorRepository.GetById(request.Id);

        if (proveedor == null)
            throw new KeyNotFoundException($"Proveedor con ID {request.Id} no encontrado");

        var response = new ProveedorResponseDto
        {
            Id = proveedor.Id,
            Nombre = proveedor.Nombre,
            Email = proveedor.Email,
            Telefono = proveedor.Telefono
        };

        return response;
    }
}