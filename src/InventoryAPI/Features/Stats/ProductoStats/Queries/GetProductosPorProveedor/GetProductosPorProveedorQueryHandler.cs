using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosPorProveedor;

public class GetProductosPorProveedorQueryHandler : IRequestHandler<GetProductosPorProveedorQuery, IEnumerable<DistribucionProveedorDto>>
{
    private readonly IProductoRepository _productoRepository;

    public GetProductosPorProveedorQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<IEnumerable<DistribucionProveedorDto>> Handle(GetProductosPorProveedorQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetProductosPorProveedorAsync();
    }

}
