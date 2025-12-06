using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Repositories;
using InventoryAPI.UnitOfWork;
using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.Productos.Commands.UpdateProducto;

public class UpdateProductoCommandHandler : IRequestHandler<UpdateProductoCommand, ProductoResponseDto>
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductoCommandHandler(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository, IProveedorRepository proveedorRepository, IUnitOfWork unitOfWork)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
        _proveedorRepository = proveedorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductoResponseDto> Handle(UpdateProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = await _productoRepository.GetById(request.Id);

        if (producto == null)
            throw new KeyNotFoundException($"Producto con ID {request.Id} no encontrado");

        if (request.Nombre != null)
            producto.Nombre = request.Nombre;

        if (request.Descripcion != null)
            producto.Descripcion = request.Descripcion;

        if (request.CategoriaId.HasValue)
        {
            var categoriaExiste = await _categoriaRepository.GetById(request.CategoriaId.Value);
            if (categoriaExiste == null)
                throw new ArgumentException("La categoría asignada no existe");

            producto.CategoriaId = request.CategoriaId.Value;
        }

        if (request.ProveedorId.HasValue)
        {
            var proveedorExiste = await _proveedorRepository.GetById(request.ProveedorId.Value);
            if (proveedorExiste == null)
                throw new ArgumentException("El proveedor asignado no existe");

            producto.ProveedorId = request.ProveedorId.Value;
        }

        if (request.StockActual.HasValue)
            producto.StockActual = request.StockActual.Value;

        if (request.StockMinimo.HasValue)
            producto.StockMinimo = request.StockMinimo.Value;

        if (request.Precio.HasValue)
            producto.Precio = request.Precio.Value;

        var productoActualizado = await _productoRepository.Update(producto);

        await _unitOfWork.SaveChangesAsync();

        var categoria = await _categoriaRepository.GetById(productoActualizado!.CategoriaId);

        Proveedor? proveedor = null;
        if (productoActualizado.ProveedorId.HasValue)
        {
            proveedor = await _proveedorRepository.GetById(productoActualizado.ProveedorId.Value);
        }

        var response = new ProductoResponseDto
        {
            Id = productoActualizado.Id,
            Nombre = productoActualizado.Nombre,
            Descripcion = productoActualizado.Descripcion,
            SKU = productoActualizado.SKU,
            CategoriaId = productoActualizado.CategoriaId,
            CategoriaNombre = categoria?.Nombre ?? string.Empty,
            StockActual = productoActualizado.StockActual,
            Precio = productoActualizado.Precio,
            ProveedorId = productoActualizado.ProveedorId,
            ProveedorNombre = productoActualizado.Proveedor?.Nombre
        };

        return response;
    }
}