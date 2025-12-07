using InventoryAPI.Data;
using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Dtos.StatsDtos.ProveedoresStatsDto;
using InventoryAPI.Models;
using InventoryAPI.Dtos.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Repositories;

public class ProveedorRepository : IProveedorRepository
{
    private readonly ApplicationDbContext _context;

    public ProveedorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Proveedor> Add(Proveedor proveedor)
    {
        _context.Proveedores.Add(proveedor);
        await _context.SaveChangesAsync();

        return proveedor;
    }

    public async Task<bool> Delete(int id)
    {
        var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.Id == id);

        if (proveedor == null)
            return false;

        _context.Proveedores.Remove(proveedor);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Proveedor?> GetById(int id)
    {
        return await _context.Proveedores.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<PagedResponse<DistribucionProveedorDto>> GetProductosPorProveedorAsync(
        int pageNumber,
        int pageSize
    )
    {
        var query = _context.Productos
                         .Include(p => p.Proveedor)
                         .AsNoTracking()
                         .Where(p => p.ProveedorId != null)
                         .GroupBy(p => p.Proveedor)
                         .Select(g => new DistribucionProveedorDto
                         {
                             NombreProveedor = g.Key!.Nombre,
                             CantidadProductos = g.Count()
                         });

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<DistribucionProveedorDto>(items, totalCount, pageNumber, pageSize);
    }

    public Task<ProveedorResponseDto?> GetProveedorMasActivoAsync()
    {
        return _context.Proveedores
                        .AsNoTracking()
                        .OrderByDescending(p => p.MovimientosStock.Count)
                        .Select(p => new ProveedorResponseDto
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Telefono = p.Telefono,
                            Email = p.Email
                        })
                        .FirstOrDefaultAsync();
    }

    public Task<int> GetTotalProveedoresAsync()
    {
        return _context.Proveedores.CountAsync();
    }

    public async Task<PagedResponse<DistribucionValorProveedorDto>> GetValorInventarioPorProveedorAsync(
        int pageNumber, int pageSize
    )
    {
        var query = _context.Productos
                         .Include(p => p.Proveedor)
                         .AsNoTracking()
                         .Where(p => p.ProveedorId != null)
                         .GroupBy(p => p.Proveedor)
                         .Select(g => new DistribucionValorProveedorDto
                         {
                             NombreProveedor = g.Key!.Nombre,
                             ValorTotal = g.Sum(p => p.Precio * p.StockActual)
                         });


        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<DistribucionValorProveedorDto>(items, totalCount, pageNumber, pageSize);
    }


    public async Task<Proveedor?> Update(Proveedor proveedor)
    {
        _context.Update(proveedor);
        await _context.SaveChangesAsync();
        return proveedor;
    }

    public async Task<PagedResponse<ProveedorResponseDto>> GetAllPaginated(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Proveedores.CountAsync();

        var proveedores = await _context.Proveedores
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = proveedores.Select(p => new ProveedorResponseDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Email = p.Email,
            Telefono = p.Telefono
        }).ToList();

        return new PagedResponse<ProveedorResponseDto>(items, totalCount, pageNumber, pageSize);
    }
}