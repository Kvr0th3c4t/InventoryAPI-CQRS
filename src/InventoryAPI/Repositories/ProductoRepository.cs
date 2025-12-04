using InventoryAPI.Data;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly ApplicationDbContext _context;

    public ProductoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Producto> Add(Producto producto)
    {
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return producto;
    }

    public async Task<bool> Delete(int id)
    {
        var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null)
            return false;

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Producto>> GetAll()
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .ToListAsync();
    }

    public async Task<Producto?> GetById(int id)
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<decimal> GetPrecioMasAltoAsync()
    {
        return _context.Productos
                        .AsNoTracking()
                        .MaxAsync(p => p.Precio);
    }

    public Task<decimal> GetPrecioMasBajoAsync()
    {
        return _context.Productos
                        .AsNoTracking()
                        .MinAsync(p => p.Precio);
    }

    public Task<decimal> GetPrecioPromedioAsync()
    {
        return _context.Productos
                        .AsNoTracking()
                        .AverageAsync(p => p.Precio);
    }

    public Task<List<DistribucionCategoriaDto>> GetProductosPorCategoriaAsync()
    {
        return _context.Productos
                        .AsNoTracking()
                        .GroupBy(p => p.Categoria)
                        .Select(g => new DistribucionCategoriaDto
                        {
                            NombreCategoria = g.Key!.Nombre,
                            CantidadProductos = g.Count(),
                            ValorTotal = g.Sum(p => p.Precio * p.StockActual)

                        })
                        .ToListAsync();
    }

    public Task<List<DistribucionProveedorDto>> GetProductosPorProveedorAsync()
    {
        return _context.Productos
                        .AsNoTracking()
                        .Include(p => p.Proveedor)
                        .Where(p => p.ProveedorId != null)
                        .GroupBy(p => p.Proveedor)
                        .Select(pr => new DistribucionProveedorDto
                        {
                            NombreProveedor = pr.Key!.Nombre,
                            CantidadProductos = pr.Count()
                        })
                        .ToListAsync();
    }

    public Task<int> GetProductosSinStockAsync()
    {
        return _context.Productos
                    .AsNoTracking()
                    .CountAsync(p => p.StockActual == 0);
    }

    public Task<int> GetProductosStockBajoAsync()
    {
        return _context.Productos
                    .AsNoTracking()
                    .CountAsync(p => p.StockActual <= p.StockMinimo);
    }

    public Task<int> GetProductosUltimos30DiasAsync()
    {
        var fechaLimite = DateTime.Now.AddDays(-30);

        return _context.Productos
                        .AsNoTracking()
                        .CountAsync(p => p.FechaCreacion >= fechaLimite);
    }

    public Task<List<ProductoResponseDto>> GetTop5MasStockAsync()
    {
        return _context.Productos
                       .AsNoTracking()
                       .OrderByDescending(p => p.StockActual)
                       .Take(5)
                       .Select(p => new ProductoResponseDto
                       {
                           Id = p.Id,
                           Nombre = p.Nombre,
                           Descripcion = p.Descripcion,
                           SKU = p.SKU,
                           CategoriaId = p.CategoriaId,
                           CategoriaNombre = p.Categoria.Nombre,
                           StockActual = p.StockActual,
                           Precio = p.Precio
                       })
                        .ToListAsync();
    }

    public Task<List<ProductoResponseDto>> GetTop5MasValiososAsync()
    {
        return _context.Productos
                        .AsNoTracking()
                        .OrderByDescending(p => p.Precio * p.StockActual)
                        .Take(5)
                        .Select(p => new ProductoResponseDto
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Descripcion = p.Descripcion,
                            SKU = p.SKU,
                            CategoriaId = p.CategoriaId,
                            CategoriaNombre = p.Categoria.Nombre,
                            StockActual = p.StockActual,
                            Precio = p.Precio
                        })
                         .ToListAsync();

    }

    public Task<List<ProductoResponseDto>> GetTop5MenosStockAsync()
    {
        return _context.Productos
                        .AsNoTracking()
                        .Where(p => p.StockActual > 0)
                        .OrderBy(p => p.StockActual)
                        .Take(5)
                        .Select(p => new ProductoResponseDto
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Descripcion = p.Descripcion,
                            SKU = p.SKU,
                            CategoriaId = p.CategoriaId,
                            CategoriaNombre = p.Categoria.Nombre,
                            StockActual = p.StockActual,
                            Precio = p.Precio
                        })
                         .ToListAsync();
    }

    public Task<int> GetTotalProductosAsync()
    {
        return _context.Productos
                    .AsNoTracking()
                    .CountAsync();
    }


    public async Task<decimal> GetValorTotalInventarioAsync()
    {
        return await _context.Productos
                        .AsNoTracking()
                        .Select(p => p.StockActual * p.Precio)
                        .SumAsync();
    }


    public async Task<Producto?> Update(Producto producto)
    {
        _context.Update(producto);
        await _context.SaveChangesAsync();
        return producto;
    }
}