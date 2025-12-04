using InventoryAPI.Dtos.StatsDtos.ProveedoresStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProveedorStats.Queries.GetValorInventarioPorProveedor;

public class GetValorInventarioPorProveedorQueryHandler : IRequestHandler<GetValorInventarioPorProveedorQuery, IEnumerable<DistribucionValorProveedorDto>>
{
    private readonly IProveedorRepository _proveedorRepository;
    public GetValorInventarioPorProveedorQueryHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<IEnumerable<DistribucionValorProveedorDto>> Handle(GetValorInventarioPorProveedorQuery request, CancellationToken cancellationToken)
    {
        return await _proveedorRepository.GetValorInventarioPorProveedorAsync();
    }
}