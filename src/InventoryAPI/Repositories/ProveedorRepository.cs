using InventoryAPI.Data;
using InventoryAPI.Models;
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

    public async Task<List<Proveedor>> GetAll()
    {
        return await _context.Proveedores.ToListAsync();
    }

    public async Task<Proveedor?> GetById(int id)
    {
        return await _context.Proveedores.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Proveedor?> Update(Proveedor proveedor)
    {
        _context.Update(proveedor);
        await _context.SaveChangesAsync();
        return proveedor;
    }
}