using InventoryAPI.Models;

namespace InventoryAPI.Repositories;

public interface IMovimientoStockRepository
{
    Task<MovimientoStock> Add(MovimientoStock movimiento);
    Task<MovimientoStock?> GetById(int id);
    Task<List<MovimientoStock>> GetAll();
}