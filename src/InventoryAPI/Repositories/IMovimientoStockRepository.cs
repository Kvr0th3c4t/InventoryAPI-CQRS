using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Models;
using InventoryAPI.Enums;

namespace InventoryAPI.Repositories;

public interface IMovimientoStockRepository
{
    Task<MovimientoStock> Add(MovimientoStock movimiento);
    Task<MovimientoStock?> GetById(int id);
    Task<int> GetTotalMovimientosUltimos30DiasAsync();
    Task<EntradasVsSalidasDto> GetEntradasVsSalidasUltimos30DiasAsync();
    Task<PagedResponse<MovimientoPorDiaDto>> GetMovimientosPorDiaAsync(int pageNumber, int pageSize);
    Task<List<ProductoMasMovidoDto>> GetProductosMasMovidosAsync();
    Task<PagedResponse<TipoMovimientoDto>> GetTipoMovimientosAsync(int pageNumber, int pageSize);
    Task<PagedResponse<MovimientoPorProveedorDto>> GetMovimientosPorProveedorAsync(int pageNumber, int pageSize);
    Task<ProductoResponseDto?> GetProductoConMasAjustesAsync();
    Task<bool> ExistsMovimientosByProveedorAsync(int proveedorId);
    Task<PagedResponse<MovimientoStockResponseDto>> GetAllPaginated(DateTimeOffset? fechaDesde, DateTimeOffset? fechaHasta, TipoMovimiento? tipo, int? productoId, string orderBy, string order, int page, int pageSize);
}