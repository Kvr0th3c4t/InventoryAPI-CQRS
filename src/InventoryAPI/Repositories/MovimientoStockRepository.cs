using InventoryAPI.Data;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using InventoryAPI.Enums;
using InventoryAPI.Models;
using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.MovimientoStockDtos;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Repositories;

public class MovimientoStockRepository : IMovimientoStockRepository
{
    private readonly ApplicationDbContext _context;

    public MovimientoStockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MovimientoStock> Add(MovimientoStock movimiento)
    {
        _context.MovimientosStock.Add(movimiento);
        await _context.SaveChangesAsync();

        return movimiento;
    }

    public async Task<List<MovimientoStock>> GetAll()
    {
        return await _context.MovimientosStock
            .Include(m => m.Producto)
            .Include(m => m.Proveedor)
            .ToListAsync();
    }

    public async Task<MovimientoStock?> GetById(int id)
    {
        return await _context.MovimientosStock
            .Include(m => m.Producto)
            .Include(m => m.Proveedor)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<EntradasVsSalidasDto> GetEntradasVsSalidasUltimos30DiasAsync()
    {
        var fechaLimite = DateTime.Now.AddDays(-30);
        var movimientoMas = await _context.MovimientosStock
                                    .AsNoTracking()
                                    .CountAsync(m => m.FechaMovimiento >= fechaLimite && (m.Tipo == TipoMovimiento.Entrada || m.Tipo == TipoMovimiento.AjustePositivo));
        var movimientoMenos = await _context.MovimientosStock
                                    .AsNoTracking()
                                    .CountAsync(m => m.FechaMovimiento >= fechaLimite && (m.Tipo == TipoMovimiento.Salida || m.Tipo == TipoMovimiento.AjusteNegativo));

        return new EntradasVsSalidasDto
        {
            TotalEntradas = movimientoMas,
            TotalSalidas = movimientoMenos
        };
    }

    public Task<List<MovimientoPorDiaDto>> GetMovimientosPorDiaAsync()
    {
        return _context.MovimientosStock
                        .AsNoTracking()
                        .GroupBy(m => m.FechaMovimiento.Date)
                        .Select(g => new MovimientoPorDiaDto
                        {
                            Fecha = g.Key,
                            TotalMovimientos = g.Count()
                        })
                        .ToListAsync();
    }

    public Task<List<MovimientoPorProveedorDto>> GetMovimientosPorProveedorAsync()
    {

        return _context.MovimientosStock
                        .Include(m => m.Proveedor)
                        .Where(m => m.ProveedorId != null)
                        .AsNoTracking()
                        .GroupBy(m => m.Proveedor)
                        .Select(g => new MovimientoPorProveedorDto
                        {
                            NombreProveedor = g.Key.Nombre,
                            TotalMovimientos = g.Count()
                        })
                        .ToListAsync();
    }

    public Task<ProductoResponseDto?> GetProductoConMasAjustesAsync()
    {
        return _context.MovimientosStock
                        .Include(m => m.Producto)
                        .ThenInclude(p => p.Categoria)
                        .AsNoTracking()
                        .Where(m => m.Tipo == TipoMovimiento.AjusteNegativo || m.Tipo == TipoMovimiento.AjustePositivo)
                        .GroupBy(m => m.Producto)
                        .OrderByDescending(g => g.Count())
                        .Select(g => new ProductoResponseDto
                        {
                            Id = g.Key.Id,
                            Nombre = g.Key.Nombre,
                            Descripcion = g.Key.Descripcion,
                            SKU = g.Key.SKU,
                            CategoriaId = g.Key.CategoriaId,
                            CategoriaNombre = g.Key.Categoria.Nombre,
                            StockActual = g.Key.StockActual,
                            Precio = g.Key.Precio
                        })
                        .FirstOrDefaultAsync();
    }

    public Task<List<ProductoMasMovidoDto>> GetProductosMasMovidosAsync()
    {
        return _context.MovimientosStock
                        .Include(m => m.Producto)
                        .AsNoTracking()
                        .GroupBy(m => m.Producto)
                        .OrderByDescending(g => g.Count())
                        .Take(5)
                        .Select(g => new ProductoMasMovidoDto
                        {
                            ProductoId = g.Key.Id,
                            NombreProducto = g.Key.Nombre,
                            TotalMovimientos = g.Count()
                        })
                        .ToListAsync();
    }

    public Task<List<TipoMovimientoDto>> GetTipoMovimientosAsync()
    {
        return _context.MovimientosStock
                        .AsNoTracking()
                        .GroupBy(m => m.Tipo)
                        .Select(g => new TipoMovimientoDto
                        {
                            TipoMovimiento = g.Key.ToString(),
                            Cantidad = g.Count()
                        })
                        .ToListAsync();
    }

    public Task<int> GetTotalMovimientosUltimos30DiasAsync()
    {
        var fechaLimite = DateTime.Now.AddDays(-30);

        return _context.MovimientosStock
                        .AsNoTracking()
                        .Where(m => m.FechaMovimiento >= fechaLimite)
                        .CountAsync();
    }

    public async Task<PagedResponse<MovimientoStockResponseDto>> GetAllPaginated(int page, int pageSize)
    {
        var totalCount = await _context.MovimientosStock.CountAsync();

        var movimientosStock = await _context.MovimientosStock
            .Include(m => m.Producto)
            .Include(m => m.Proveedor)
            .OrderBy(m => m.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = movimientosStock.Select(m => new MovimientoStockResponseDto
        {
            Id = m.Id,
            ProductoId = m.Producto.Id,
            ProductoNombre = m.Producto.Nombre,
            ProveedorNombre = m.Proveedor.Nombre,
            Tipo = m.Tipo,
            Cantidad = m.Cantidad,
            Razon = m.Razon,
            Fecha = m.FechaMovimiento
        }).ToList();

        return new PagedResponse<MovimientoStockResponseDto>(items, totalCount, page, pageSize);
    }
}