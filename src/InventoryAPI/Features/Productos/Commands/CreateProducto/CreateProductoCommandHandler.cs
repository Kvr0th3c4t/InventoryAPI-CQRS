using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Productos.Commands.CreateProducto;

public class CreateProductoCommandHandler : IRequestHandler<CreateProductoCommand, ProductoResponseDto>
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public CreateProductoCommandHandler(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<ProductoResponseDto> Handle(CreateProductoCommand request, CancellationToken cancellationToken)
    {
        // Validaciones
        if (request.Precio < 0)
            throw new ArgumentException("El precio no puede ser negativo");

        if (request.StockActual < 0)
            throw new ArgumentException("El stock actual no puede ser negativo");

        if (request.StockMinimo < 0)
            throw new ArgumentException("El stock mínimo no puede ser negativo");

        var categoriaBuscada = await _categoriaRepository.GetById(request.CategoriaId);
        if (categoriaBuscada == null)
            throw new ArgumentException("La categoría asignada no existe");

        // Generar SKU
        string sku = "PROD-" + Guid.NewGuid().ToString("N")[..8].ToUpper();

        // Crear producto
        var producto = new Producto
        {
            Nombre = request.Nombre,
            SKU = sku,
            CategoriaId = request.CategoriaId,
            Descripcion = request.Descripcion,
            StockActual = request.StockActual,
            StockMinimo = request.StockMinimo,
            Precio = request.Precio,
            FechaCreacion = DateTime.Now
        };

        var productoCreado = await _productoRepository.Add(producto);

        // Construir respuesta
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