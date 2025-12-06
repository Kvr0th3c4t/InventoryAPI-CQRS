using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Productos.Queries.GetProductoById;

public class GetProductoByIdQueryHandler : IRequestHandler<GetProductoByIdQuery, ProductoResponseDto>
{
    private readonly IProductoRepository _productoRepository;

    public GetProductoByIdQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<ProductoResponseDto> Handle(GetProductoByIdQuery request, CancellationToken cancellationToken)
    {
        var producto = await _productoRepository.GetById(request.Id);

        if (producto == null)
            throw new KeyNotFoundException($"Producto con ID {request.Id} no encontrado");

        if (producto.Categoria == null)
            throw new KeyNotFoundException("Datos corruptos: categoría no existe");

        var response = new ProductoResponseDto
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            SKU = producto.SKU,
            CategoriaId = producto.CategoriaId,
            CategoriaNombre = producto.Categoria.Nombre,
            StockActual = producto.StockActual,
            Precio = producto.Precio,
            ProveedorId = producto.ProveedorId,
            ProveedorNombre = producto.Proveedor?.Nombre
        };

        return response;
    }
}