using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.CategoriaStats.Queries.GetCategoriaConMayorValor;

public class GetCategoriaConMayorValorQueryHandler : IRequestHandler<GetCategoriaConMayorValorQuery, CategoriaResponseDto?>
{
    private readonly ICategoriaRepository _categoriaRepository;
    public GetCategoriaConMayorValorQueryHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<CategoriaResponseDto?> Handle(GetCategoriaConMayorValorQuery request, CancellationToken cancellationToken)
    {
        return await _categoriaRepository.GetCategoriaConMayorValorAsync();
    }

}