using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosPorCategoria;

public class GetProductosPorCategoriaQueryHandler : IRequestHandler<GetProductosPorCategoriaQuery, IEnumerable<DistribucionCategoriaDto>>
{
    private readonly IProductoRepository _productoRepository;

    public GetProductosPorCategoriaQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<IEnumerable<DistribucionCategoriaDto>> Handle(GetProductosPorCategoriaQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetProductosPorCategoriaAsync();
    }

}
