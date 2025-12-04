using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProveedorStats.Queries.GetProductosPorProveedor;

public class GetProductosPorProveedorQueryHandler : IRequestHandler<GetProductosPorProveedorQuery, IEnumerable<DistribucionProveedorDto>>
{
    private readonly IProveedorRepository _proveedorRepository;
    public GetProductosPorProveedorQueryHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<IEnumerable<DistribucionProveedorDto>> Handle(GetProductosPorProveedorQuery request, CancellationToken cancellationToken)
    {
        return await _proveedorRepository.GetProductosPorProveedorAsync();
    }
}