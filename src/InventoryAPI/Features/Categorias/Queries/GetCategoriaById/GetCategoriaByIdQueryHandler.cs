using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Categorias.Queries.GetCategoriaById;

public class GetCategoriaByIdQueryHandler : IRequestHandler<GetCategoriaByIdQuery, CategoriaResponseDto>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public GetCategoriaByIdQueryHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<CategoriaResponseDto> Handle(GetCategoriaByIdQuery request, CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepository.GetById(request.Id);

        if (categoria == null)
            throw new KeyNotFoundException($"Categoría con ID {request.Id} no encontrada");

        var response = new CategoriaResponseDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion
        };

        return response;
    }
}