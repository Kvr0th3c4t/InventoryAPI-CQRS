using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using InventoryAPI.Models;

namespace InventoryAPI.Repositories;

public interface IMovimientoStockRepository
{
    Task<MovimientoStock> Add(MovimientoStock movimiento);
    Task<MovimientoStock?> GetById(int id);
    Task<List<MovimientoStock>> GetAll();
    Task<int> GetTotalMovimientosUltimos30DiasAsync();
    Task<EntradasVsSalidasDto> GetEntradasVsSalidasUltimos30DiasAsync();
    Task<List<MovimientoPorDiaDto>> GetMovimientosPorDiaAsync();
    Task<List<ProductoMasMovidoDto>> GetProductosMasMovidosAsync();
    Task<List<TipoMovimientoDto>> GetTipoMovimientosAsync();
    Task<List<MovimientoPorProveedorDto>> GetMovimientosPorProveedorAsync();
    Task<ProductoResponseDto?> GetProductoConMasAjustesAsync();
}