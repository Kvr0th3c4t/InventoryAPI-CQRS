using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Categorias.Commands.CreateCategoria;

public class CreateCategoriaCommandHandler : IRequestHandler<CreateCategoriaCommand, CategoriaResponseDto>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CreateCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<CategoriaResponseDto> Handle(CreateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = new Categoria
        {
            Nombre = request.Nombre,
            Descripcion = request.Descripcion
        };

        var categoriaCreada = await _categoriaRepository.Add(categoria);

        var response = new CategoriaResponseDto
        {
            Id = categoriaCreada.Id,
            Nombre = categoriaCreada.Nombre,
            Descripcion = categoriaCreada.Descripcion
        };

        return response;
    }
}