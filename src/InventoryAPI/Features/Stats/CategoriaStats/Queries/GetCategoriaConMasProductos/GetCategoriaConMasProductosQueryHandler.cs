using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.CategoriaStats.Queries.GetCategoriaConMasProductos;

public class GetCategoriaConMasProductosQueryHandler : IRequestHandler<GetCategoriaConMasProductosQuery, CategoriaResponseDto?>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public GetCategoriaConMasProductosQueryHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<CategoriaResponseDto?> Handle(GetCategoriaConMasProductosQuery request, CancellationToken cancellationToken)
    {
        return await _categoriaRepository.GetCategoriaConMasProductosAsync();
    }
}