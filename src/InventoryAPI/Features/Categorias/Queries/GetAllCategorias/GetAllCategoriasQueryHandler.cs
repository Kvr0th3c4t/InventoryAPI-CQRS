using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Categorias.Queries.GetAllCategorias;

public class GetAllCategoriasQueryHandler : IRequestHandler<GetAllCategoriasQuery, IEnumerable<CategoriaResponseDto>>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public GetAllCategoriasQueryHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<IEnumerable<CategoriaResponseDto>> Handle(GetAllCategoriasQuery request, CancellationToken cancellationToken)
    {
        var categorias = await _categoriaRepository.GetAll();

        var result = categorias.Select(c => new CategoriaResponseDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Descripcion = c.Descripcion
        });

        return result;
    }
}