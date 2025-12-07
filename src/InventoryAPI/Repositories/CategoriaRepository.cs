using InventoryAPI.Data;
using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Models;
using InventoryAPI.Dtos.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly ApplicationDbContext _context;

    public CategoriaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Categoria> Add(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return categoria;
    }

    public async Task<bool> Delete(int id)
    {
        var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null)
            return false;

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Categoria?> GetById(int id)
    {
        return await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<CategoriaResponseDto?> GetCategoriaConMasProductosAsync()
    {
        return _context.Categorias
                        .AsNoTracking()
                        .OrderByDescending(c => c.Productos.Count())
                        .Select(c => new CategoriaResponseDto
                        {
                            Id = c.Id,
                            Nombre = c.Nombre,
                            Descripcion = c.Descripcion
                        })
                        .FirstOrDefaultAsync();
    }

    public Task<CategoriaResponseDto?> GetCategoriaConMayorValorAsync()
    {
        return _context.Productos
                    .AsNoTracking()
                    .GroupBy(p => p.Categoria)
                    .Select(g => new
                    {
                        Categoria = g.Key,
                        ValorTotal = g.Sum(p => p.Precio * p.StockActual)
                    })
                    .OrderByDescending(x => x.ValorTotal)
                    .Select(x => new CategoriaResponseDto
                    {
                        Id = x.Categoria!.Id,
                        Nombre = x.Categoria.Nombre,
                        Descripcion = x.Categoria.Descripcion
                    })
                    .FirstOrDefaultAsync();
    }

    public async Task<PagedResponse<DistribucionCategoriaDto>> GetDistribucionProductosPorCategoriaAsync(
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

    public Task<int> GetTotalCategoriasAsync()
    {
        return _context.Categorias
                        .AsNoTracking()
                        .CountAsync();
    }

    public async Task<Categoria?> Update(Categoria categoria)
    {
        _context.Update(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }
    public async Task<PagedResponse<CategoriaResponseDto>> GetAllPaginated(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Categorias.CountAsync();

        var categorias = await _context.Categorias
            .OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = categorias.Select(c => new CategoriaResponseDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Descripcion = c.Descripcion
        }).ToList();

        return new PagedResponse<CategoriaResponseDto>(items, totalCount, pageNumber, pageSize);
    }
}