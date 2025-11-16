using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Commands.DeleteProveedor;

public class DeleteProveedorCommandHandler : IRequestHandler<DeleteProveedorCommand, bool>
{
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IProductoRepository _productosRepository;
    private readonly IMovimientoStockRepository _movimientosRepository;

    public DeleteProveedorCommandHandler(IProveedorRepository proveedorRepository, IProductoRepository productoRepository, IMovimientoStockRepository movimientoStockRepository)
    {
        _proveedorRepository = proveedorRepository;
        _productosRepository = productoRepository;
        _movimientosRepository = movimientoStockRepository;
    }

    public Task<bool> Handle(DeleteProveedorCommand request, CancellationToken cancellationToken)
    {
        var proveedor = _proveedorRepository.GetById(request.Id);

        if (proveedor == null)
            throw new KeyNotFoundException($"No se puede eliminar. El proveedor con Id {request.Id} no existe");

        var tieneProductos = _productosRepository.GetAll()
            .Any(p => p.ProveedorId == proveedor.Id);

        if (tieneProductos)
        {
            throw new InvalidOperationException(
                "No se puede eliminar: el proveedor tiene productos asociados");
        }


        var tieneMovimientos = _movimientosRepository.GetAll()
            .Any(m => m.ProveedorId == proveedor.Id);


        if (tieneMovimientos)
        {
            throw new InvalidOperationException(
                "No se puede eliminar: el proveedor tiene movimientos de stock asociados");
        }
        _proveedorRepository.Delete(request.Id);

        return Task.FromResult(true);
    }
}