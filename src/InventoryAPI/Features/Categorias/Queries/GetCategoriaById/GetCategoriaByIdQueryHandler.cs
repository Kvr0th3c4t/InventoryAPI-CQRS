using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Categorias.Queries.GetCategoriaById;

public class GetCategoriaByIdQueryHandler : IRequestHandler<GetCategoriaByIdQuery, CategoriaResponseDto>
{
    private readonly ICategoriaRepository _categoriarepository;

    public GetCategoriaByIdQueryHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriarepository = categoriaRepository;
    }

    public Task<CategoriaResponseDto> Handle(GetCategoriaByIdQuery request, CancellationToken cancellationToken)
    {

        var categoria = _categoriarepository.GetById(request.Id);
        if (categoria == null)
            throw new KeyNotFoundException("La categoría no existe");

        var response = new CategoriaResponseDto
        {
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion,
            Id = categoria.Id
        };

        return Task.FromResult(response);
    }
}