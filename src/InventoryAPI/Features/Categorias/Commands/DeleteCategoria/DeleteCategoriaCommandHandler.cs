using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Categorias.Commands.DeleteCategoria;

public class DeleteCategoriaCommandHandler : IRequestHandler<DeleteCategoriaCommand, bool>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public DeleteCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<bool> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepository.GetById(request.Id);

        if (categoria == null)
            throw new KeyNotFoundException($"Categoría con ID {request.Id} no encontrada");

        return await _categoriaRepository.Delete(request.Id);
    }
}