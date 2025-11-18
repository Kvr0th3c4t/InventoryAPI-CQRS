using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Categorias.Commands.UpdateCategoria;

public class UpdateCategoriaCommandHandler : IRequestHandler<UpdateCategoriaCommand, CategoriaResponseDto>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public UpdateCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<CategoriaResponseDto> Handle(UpdateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepository.GetById(request.Id);

        if (categoria == null)
            throw new KeyNotFoundException($"Categoría con ID {request.Id} no encontrada");

        // Actualización parcial
        if (request.Nombre != null)
            categoria.Nombre = request.Nombre;

        if (request.Descripcion != null)
            categoria.Descripcion = request.Descripcion;

        var categoriaActualizada = await _categoriaRepository.Update(categoria);

        var response = new CategoriaResponseDto
        {
            Id = categoriaActualizada!.Id,
            Nombre = categoriaActualizada.Nombre,
            Descripcion = categoriaActualizada.Descripcion
        };

        return response;
    }
}