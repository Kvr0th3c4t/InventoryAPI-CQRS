using InventoryAPI.Repositories;
using InventoryAPI.UnitOfWork;
using MediatR;

namespace InventoryAPI.Features.Categorias.Commands.DeleteCategoria;

public class DeleteCategoriaCommandHandler : IRequestHandler<DeleteCategoriaCommand, bool>
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoriaCommandHandler(ICategoriaRepository categoriaRepository, IUnitOfWork unitOfWork)
    {
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepository.GetById(request.Id);

        if (categoria == null)
            throw new KeyNotFoundException($"Categoría con ID {request.Id} no encontrada");

        await _categoriaRepository.Delete(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}