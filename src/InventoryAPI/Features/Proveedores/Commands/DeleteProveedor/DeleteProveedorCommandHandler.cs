using InventoryAPI.Repositories;
using InventoryAPI.UnitOfWork;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Commands.DeleteProveedor;

public class DeleteProveedorCommandHandler : IRequestHandler<DeleteProveedorCommand, bool>
{
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IMovimientoStockRepository _movimientoStockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProveedorCommandHandler(
        IProveedorRepository proveedorRepository,
        IMovimientoStockRepository movimientoStockRepository,
        IUnitOfWork unitOfWork)
    {
        _proveedorRepository = proveedorRepository;
        _movimientoStockRepository = movimientoStockRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteProveedorCommand request, CancellationToken cancellationToken)
    {
        var proveedor = await _proveedorRepository.GetById(request.Id);

        if (proveedor == null)
            throw new KeyNotFoundException($"Proveedor con ID {request.Id} no encontrado");

        var tieneMovimientos = await _movimientoStockRepository
            .ExistsMovimientosByProveedorAsync(request.Id);

        if (tieneMovimientos)
            throw new InvalidOperationException(
                "No se puede eliminar el proveedor porque tiene movimientos de stock asociados");

        await _proveedorRepository.Delete(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}