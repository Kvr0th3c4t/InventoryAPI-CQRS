using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Commands.DeleteProveedor;

public class DeleteProveedorCommandHandler : IRequestHandler<DeleteProveedorCommand, bool>
{
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IMovimientoStockRepository _movimientoStockRepository;

    public DeleteProveedorCommandHandler(IProveedorRepository proveedorRepository, IMovimientoStockRepository movimientoStockRepository)
    {
        _proveedorRepository = proveedorRepository;
        _movimientoStockRepository = movimientoStockRepository;
    }

    public async Task<bool> Handle(DeleteProveedorCommand request, CancellationToken cancellationToken)
    {
        var proveedor = await _proveedorRepository.GetById(request.Id);

        if (proveedor == null)
            throw new KeyNotFoundException($"Proveedor con ID {request.Id} no encontrado");

        // Verificar que no tenga movimientos de stock
        var movimientos = await _movimientoStockRepository.GetAll();
        var tieneMovimientos = movimientos.Any(m => m.ProveedorId == request.Id);

        if (tieneMovimientos)
            throw new InvalidOperationException("No se puede eliminar el proveedor porque tiene movimientos de stock asociados");

        return await _proveedorRepository.Delete(request.Id);
    }
}