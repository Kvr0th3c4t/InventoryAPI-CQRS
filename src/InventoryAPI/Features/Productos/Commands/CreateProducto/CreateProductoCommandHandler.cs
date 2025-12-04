using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using InventoryAPI.UnitOfWork;
using MediatR;

namespace InventoryAPI.Features.Productos.Commands.CreateProducto;

public class CreateProductoCommandHandler : IRequestHandler<CreateProductoCommand, ProductoResponseDto>
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductoCommandHandler(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository, IProveedorRepository proveedorRepository, IUnitOfWork unitOfWork)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
        _proveedorRepository = proveedorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductoResponseDto> Handle(CreateProductoCommand request, CancellationToken cancellationToken)
    {

        var categoriaBuscada = await _categoriaRepository.GetById(request.CategoriaId);
        if (categoriaBuscada == null)
            throw new ArgumentException("La categoría asignada no existe");

        if (request.ProveedorId.HasValue)
        {
            var proveedorBuscado = await _proveedorRepository.GetById(request.ProveedorId.Value);
            if (proveedorBuscado == null)
                throw new ArgumentException("El proveedor asignado no existe");
        }

        // Generar SKU
        string sku = "PROD-" + Guid.NewGuid().ToString("N")[..8].ToUpper();

        var producto = new Producto
        {
            Nombre = request.Nombre,
            SKU = sku,
            CategoriaId = request.CategoriaId,
            ProveedorId = request.ProveedorId,
            Descripcion = request.Descripcion,
            StockActual = request.StockActual,
            StockMinimo = request.StockMinimo,
            Precio = request.Precio,
            FechaCreacion = DateTimeOffset.UtcNow
        };

        var productoCreado = await _productoRepository.Add(producto);

        await _unitOfWork.SaveChangesAsync();

        var response = new ProductoResponseDto
        {
            Id = productoCreado.Id,
            Nombre = productoCreado.Nombre,
            Descripcion = productoCreado.Descripcion,
            SKU = productoCreado.SKU,
            CategoriaId = productoCreado.CategoriaId,
            CategoriaNombre = categoriaBuscada.Nombre,
            StockActual = productoCreado.StockActual,
            Precio = productoCreado.Precio
        };

        return response;
    }
}