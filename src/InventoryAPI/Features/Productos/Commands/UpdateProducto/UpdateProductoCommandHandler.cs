using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Productos.Commands.UpdateProducto;

public class UpdateProductoCommandHandler : IRequestHandler<UpdateProductoCommand, ProductoResponseDto>
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public UpdateProductoCommandHandler(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<ProductoResponseDto> Handle(UpdateProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = await _productoRepository.GetById(request.Id);

        if (producto == null)
            throw new KeyNotFoundException($"Producto con ID {request.Id} no encontrado");

        // Validaciones
        if (request.Precio.HasValue && request.Precio < 0)
            throw new ArgumentException("El precio no puede ser negativo");

        if (request.StockActual.HasValue && request.StockActual < 0)
            throw new ArgumentException("El stock actual no puede ser negativo");

        if (request.StockMinimo.HasValue && request.StockMinimo < 0)
            throw new ArgumentException("El stock mínimo no puede ser negativo");

        // Actualización parcial
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

        if (request.StockActual.HasValue)
            producto.StockActual = request.StockActual.Value;

        if (request.StockMinimo.HasValue)
            producto.StockMinimo = request.StockMinimo.Value;

        if (request.Precio.HasValue)
            producto.Precio = request.Precio.Value;

        var productoActualizado = await _productoRepository.Update(producto);

        // Obtener categoría para la respuesta
        var categoria = await _categoriaRepository.GetById(productoActualizado!.CategoriaId);

        var response = new ProductoResponseDto
        {
            Id = productoActualizado.Id,
            Nombre = productoActualizado.Nombre,
            Descripcion = productoActualizado.Descripcion,
            SKU = productoActualizado.SKU,
            CategoriaId = productoActualizado.CategoriaId,
            CategoriaNombre = categoria?.Nombre ?? string.Empty,
            StockActual = productoActualizado.StockActual,
            Precio = productoActualizado.Precio
        };

        return response;
    }
}