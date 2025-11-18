using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Queries.GetAllProveedores;

public class GetAllProveedoresQueryHandler : IRequestHandler<GetAllProveedoresQuery, IEnumerable<ProveedorResponseDto>>
{
    private readonly IProveedorRepository _proveedorRepository;

    public GetAllProveedoresQueryHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<IEnumerable<ProveedorResponseDto>> Handle(GetAllProveedoresQuery request, CancellationToken cancellationToken)
    {
        var proveedores = await _proveedorRepository.GetAll();

        var result = proveedores.Select(p => new ProveedorResponseDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Email = p.Email,
            Telefono = p.Telefono
        });

        return result;
    }
}