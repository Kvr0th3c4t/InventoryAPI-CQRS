using InventoryAPI.Data;
using InventoryAPI.Models;
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
}