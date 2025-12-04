using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProveedorStats.Queries.GetTotalProveedores;

public class GetTotalProveedoresQueryHandler : IRequestHandler<GetTotalProveedoresQuery, int>
{
    private readonly IProveedorRepository _proveedorRepository;
    public GetTotalProveedoresQueryHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<int> Handle(GetTotalProveedoresQuery request, CancellationToken cancellationToken)
    {
        return await _proveedorRepository.GetTotalProveedoresAsync();
    }
}