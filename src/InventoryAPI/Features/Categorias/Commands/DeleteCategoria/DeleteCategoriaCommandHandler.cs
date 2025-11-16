using InventoryAPI.Features.Categorias.Commands.DeleteCategoria;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Categorias.DeleteCategoria;

public class DeleteCategoriaCommandHandler : IRequestHandler<DeleteCategoriaCommand, bool>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public DeleteCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }
    public Task<bool> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
    {
        var eliminado = _categoriaRepository.Delete(request.Id);

        if (!eliminado)
            throw new KeyNotFoundException($"No se puede eliminar. La categoria con Id {request.Id} no existe");

        return Task.FromResult(true);
    }
}