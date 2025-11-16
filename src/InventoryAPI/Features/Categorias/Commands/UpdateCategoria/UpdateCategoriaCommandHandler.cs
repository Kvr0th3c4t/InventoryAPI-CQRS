using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Categorias.Commands.UpdateCategoria;

public class UpdateCategoriaCommandHandler : IRequestHandler<UpdateCategoriaCommand, CategoriaResponseDto>
{
    private readonly ICategoriaRepository _categoriarepository;

    public UpdateCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriarepository = categoriaRepository;
    }

    public Task<CategoriaResponseDto> Handle(UpdateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = _categoriarepository.GetById(request.Id);

        if (categoria == null)
        {
            throw new ArgumentException("La categoría no existe");
        }

        if (request.Nombre != null) categoria.Nombre = request.Nombre;
        if (request.Descripcion != null) categoria.Descripcion = request.Descripcion;

        _categoriarepository.Update(categoria);

        var response = new CategoriaResponseDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion
        };

        return Task.FromResult(response);
    }
}