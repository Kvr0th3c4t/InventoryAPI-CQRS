using InventoryAPI.Data;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Models;
using InventoryAPI.Dtos.Pagination;
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
            .Include(p => p.Proveedor)
            .ToListAsync();
    }

    public async Task<Producto?> GetById(int id)
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .Include(p => p.Proveedor)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<decimal> GetPrecioMasAltoAsync()
    {
        return await _context.Productos
                        .AsNoTracking()
                        .MaxAsync(p => p.Precio);
    }

    public async Task<decimal> GetPrecioMasBajoAsync()
    {
        return await _context.Productos
                        .AsNoTracking()
                        .MinAsync(p => p.Precio);
    }

    public async Task<decimal> GetPrecioPromedioAsync()
    {
        return await _context.Productos
                        .AsNoTracking()
                        .AverageAsync(p => p.Precio);
    }

    public async Task<PagedResponse<DistribucionCategoriaDto>> GetProductosPorCategoriaAsync(
    int pageNumber,
    int pageSize)
    {
        var query = _context.Productos
            .AsNoTracking()
            .GroupBy(p => p.Categoria)
            .Select(g => new DistribucionCategoriaDto
            {
                NombreCategoria = g.Key!.Nombre,
                CantidadProductos = g.Count(),
                ValorTotal = g.Sum(p => p.Precio * p.StockActual)
            });

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<DistribucionCategoriaDto>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<PagedResponse<DistribucionProveedorDto>> GetProductosPorProveedorAsync(int pageNumber, int pageSize)
    {
        var query = _context.Productos
                        .AsNoTracking()
                        .Include(p => p.Proveedor)
                        .Where(p => p.ProveedorId != null)
                        .GroupBy(p => p.Proveedor)
                        .Select(pr => new DistribucionProveedorDto
                        {
                            NombreProveedor = pr.Key!.Nombre,
                            CantidadProductos = pr.Count()
                        });

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<DistribucionProveedorDto>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<int> GetProductosSinStockAsync()
    {
        return await _context.Productos
                    .AsNoTracking()
                    .CountAsync(p => p.StockActual == 0);
    }

    public async Task<int> GetProductosStockBajoAsync()
    {
        return await _context.Productos
                    .AsNoTracking()
                    .CountAsync(p => p.StockActual <= p.StockMinimo);
    }

    public async Task<int> GetProductosUltimos30DiasAsync()
    {
        var fechaLimite = DateTime.Now.AddDays(-30);

        return await _context.Productos
                        .AsNoTracking()
                        .CountAsync(p => p.FechaCreacion >= fechaLimite);
    }

    public async Task<List<ProductoResponseDto>> GetTop5MasStockAsync()
    {
        return await _context.Productos
                        .Include(p => p.Proveedor)
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
                           Precio = p.Precio,
                           ProveedorId = p.ProveedorId,
                           ProveedorNombre = p.Proveedor.Nombre
                       })
                        .ToListAsync();
    }

    public async Task<List<ProductoResponseDto>> GetTop5MasValiososAsync()
    {
        return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
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
                            Precio = p.Precio,
                            ProveedorId = p.ProveedorId,
                            ProveedorNombre = p.Proveedor.Nombre
                        })
                         .ToListAsync();

    }

    public async Task<List<ProductoResponseDto>> GetTop5MenosStockAsync()
    {
        return await _context.Productos
                        .Include(p => p.Categoria)
                        .Include(p => p.Proveedor)
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
                            Precio = p.Precio,
                            ProveedorId = p.ProveedorId,
                            ProveedorNombre = p.Proveedor.Nombre
                        })
                         .ToListAsync();
    }

    public async Task<int> GetTotalProductosAsync()
    {
        return await _context.Productos
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

    public async Task<PagedResponse<ProductoResponseDto>> GetAllPaginated(
        string? search,
        int? categoriaId,
        int? proveedorId,
        decimal? precioMin,
        decimal? precioMax,
        bool stockBajo,
        string orderBy,
        string order,
        int pageNumber,
        int pageSize)
    {
        var query = _context.Productos
            .Include(p => p.Categoria)
            .Include(p => p.Proveedor)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.Nombre.Contains(search) ||
                p.Descripcion.Contains(search) ||
                p.SKU.Contains(search));
        }

        if (categoriaId.HasValue)
        {
            query = query.Where(p => p.CategoriaId == categoriaId.Value);
        }

        if (proveedorId.HasValue)
        {
            query = query.Where(p => p.ProveedorId == proveedorId.Value);
        }

        if (precioMin.HasValue)
        {
            query = query.Where(p => p.Precio >= precioMin.Value);
        }

        if (precioMax.HasValue)
        {
            query = query.Where(p => p.Precio <= precioMax.Value);
        }

        if (stockBajo)
        {
            query = query.Where(p => p.StockActual < p.StockMinimo);
        }

        var totalCount = await query.CountAsync();

        query = orderBy switch
        {
            "precio" => order == "desc"
                ? query.OrderByDescending(p => p.Precio)
                : query.OrderBy(p => p.Precio),
            "stock" => order == "desc"
                ? query.OrderByDescending(p => p.StockActual)
                : query.OrderBy(p => p.StockActual),
            "categoria" => order == "desc"
                ? query.OrderByDescending(p => p.Categoria.Nombre)
                : query.OrderBy(p => p.Categoria.Nombre),
            "proveedor" => order == "desc"
                ? query.OrderByDescending(p => p.Proveedor == null ? 1 : 0)
                       .ThenByDescending(p => p.Proveedor.Nombre)
                : query.OrderBy(p => p.Proveedor == null ? 1 : 0)
                       .ThenBy(p => p.Proveedor.Nombre),
            _ => order == "desc"
                ? query.OrderByDescending(p => p.Nombre)
                : query.OrderBy(p => p.Nombre)
        };

        var productos = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = productos.Select(p => new ProductoResponseDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            SKU = p.SKU,
            CategoriaId = p.CategoriaId,
            CategoriaNombre = p.Categoria?.Nombre ?? string.Empty,
            StockActual = p.StockActual,
            Precio = p.Precio,
            ProveedorId = p.ProveedorId,
            ProveedorNombre = p.Proveedor?.Nombre
        }).ToList();

        return new PagedResponse<ProductoResponseDto>(items, totalCount, pageNumber, pageSize);
    }



}