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

    public Task<ProveedorResponseDto> Handle(GetProveedorByIdQuery request, CancellationToken cancellationToken)
    {
        var proveedor = _proveedorRepository.GetById(request.Id);

        if (proveedor == null)
            throw new KeyNotFoundException($"Proveedor con ID {request.Id} no encontrado");

        var result = new ProveedorResponseDto
        {
            Id = proveedor.Id,
            Nombre = proveedor.Nombre,
            Email = proveedor.Email,
            Telefono = proveedor.Telefono
        };

        return Task.FromResult(result);
    }
}