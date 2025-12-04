using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Productos.Queries.GetAllProductos;

public class GetAllProductosQueryHandler : IRequestHandler<GetAllProductosQuery, IEnumerable<ProductoResponseDto>>
{
    private readonly IProductoRepository _productoRepository;

    public GetAllProductosQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<IEnumerable<ProductoResponseDto>> Handle(GetAllProductosQuery request, CancellationToken cancellationToken)
    {
        var productos = await _productoRepository.GetAll();

        var result = productos.Select(p => new ProductoResponseDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            SKU = p.SKU,
            CategoriaId = p.CategoriaId,
            CategoriaNombre = p.Categoria?.Nombre ?? string.Empty,
            StockActual = p.StockActual,
            Precio = p.Precio
        });

        return result;
    }
}