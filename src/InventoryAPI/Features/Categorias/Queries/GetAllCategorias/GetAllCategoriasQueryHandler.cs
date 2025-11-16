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

    public Task<IEnumerable<CategoriaResponseDto>> Handle(GetAllCategoriasQuery request, CancellationToken cancellationToken)
    {
        var categorias = _categoriaRepository.GetAll();

        var result = categorias
            .Select(categoria => new CategoriaResponseDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            });

        return Task.FromResult(result);
    }
}