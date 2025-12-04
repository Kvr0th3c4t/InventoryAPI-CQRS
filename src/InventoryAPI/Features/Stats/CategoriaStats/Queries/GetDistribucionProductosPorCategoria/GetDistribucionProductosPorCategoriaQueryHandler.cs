using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.CategoriaStats.Queries.GetDistribucionProductosPorCategoria;

public class GetDistribucionProductosPorCategoriaQueryHandler : IRequestHandler<GetDistribucionProductosPorCategoriaQuery, IEnumerable<DistribucionCategoriaDto>>
{
    private readonly ICategoriaRepository _categoriaRepository;
    public GetDistribucionProductosPorCategoriaQueryHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<IEnumerable<DistribucionCategoriaDto>> Handle(GetDistribucionProductosPorCategoriaQuery request, CancellationToken cancellationToken)
    {
        return await _categoriaRepository.GetDistribucionProductosPorCategoriaAsync();
    }
}