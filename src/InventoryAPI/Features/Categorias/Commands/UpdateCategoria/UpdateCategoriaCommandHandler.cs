using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Repositories;
using InventoryAPI.UnitOfWork;
using MediatR;

namespace InventoryAPI.Features.Categorias.Commands.UpdateCategoria;

public class UpdateCategoriaCommandHandler : IRequestHandler<UpdateCategoriaCommand, CategoriaResponseDto>
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoriaCommandHandler(ICategoriaRepository categoriaRepository, IUnitOfWork unitOfWork)
    {
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
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
        await _unitOfWork.SaveChangesAsync();

        var response = new CategoriaResponseDto
        {
            Id = categoriaActualizada!.Id,
            Nombre = categoriaActualizada.Nombre,
            Descripcion = categoriaActualizada.Descripcion
        };

        return response;
    }
}