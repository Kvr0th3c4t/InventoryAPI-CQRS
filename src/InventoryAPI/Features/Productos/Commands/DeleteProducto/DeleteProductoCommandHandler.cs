using InventoryAPI.Features.Productos.DeleteProducto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Productos.Commands.DeleteProducto;

public class DeleteProductoCommandHandler : IRequestHandler<DeleteProductoCommand, bool>
{
    private readonly IProductoRepository _productoRepository;

    public DeleteProductoCommandHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<bool> Handle(DeleteProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = await _productoRepository.GetById(request.Id);

        if (producto == null)
            throw new KeyNotFoundException($"Producto con ID {request.Id} no encontrado");

        return await _productoRepository.Delete(request.Id);
    }
}