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

    public Task<IEnumerable<ProveedorResponseDto>> Handle(GetAllProveedoresQuery request, CancellationToken cancellationToken)
    {
        var proveedores = _proveedorRepository.GetAll();

        var result = proveedores
             .Select(proveedor =>
             {
                 return new ProveedorResponseDto
                 {
                     Id = proveedor.Id,
                     Nombre = proveedor.Nombre,
                     Email = proveedor.Email,
                     Telefono = proveedor.Telefono
                 };
             });
        return Task.FromResult(result);
    }
}