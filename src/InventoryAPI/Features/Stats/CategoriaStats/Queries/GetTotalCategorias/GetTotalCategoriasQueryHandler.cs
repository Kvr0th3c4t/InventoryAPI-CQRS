using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.CategoriaStats.Queries.GetTotalCategorias;

public class GetTotalCategoriasQueryHandler : IRequestHandler<GetTotalCategoriasQuery, int>
{
    private readonly ICategoriaRepository _categoriaRepository;
    public GetTotalCategoriasQueryHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<int> Handle(GetTotalCategoriasQuery request, CancellationToken cancellationToken)
    {
        return await _categoriaRepository.GetTotalCategoriasAsync();
    }
}