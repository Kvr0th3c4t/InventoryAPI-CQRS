using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using InventoryAPI.UnitOfWork;
using MediatR;

namespace InventoryAPI.Features.Categorias.Commands.CreateCategoria;

public class CreateCategoriaCommandHandler : IRequestHandler<CreateCategoriaCommand, CategoriaResponseDto>
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoriaCommandHandler(ICategoriaRepository categoriaRepository, IUnitOfWork unitOfWork)
    {
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoriaResponseDto> Handle(CreateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = new Categoria
        {
            Nombre = request.Nombre,
            Descripcion = request.Descripcion
        };

        var categoriaCreada = await _categoriaRepository.Add(categoria);
        await _unitOfWork.SaveChangesAsync();

        var response = new CategoriaResponseDto
        {
            Id = categoriaCreada.Id,
            Nombre = categoriaCreada.Nombre,
            Descripcion = categoriaCreada.Descripcion
        };

        return response;
    }
}