using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Models;
using InventoryAPI.Dtos.Pagination;

namespace InventoryAPI.Repositories;

public interface IProductoRepository : IGenericRepository<Producto>
{
    Task<int> GetTotalProductosAsync();
    Task<int> GetProductosStockBajoAsync();
    Task<int> GetProductosSinStockAsync();
    Task<decimal> GetValorTotalInventarioAsync();
    Task<List<DistribucionCategoriaDto>> GetProductosPorCategoriaAsync();
    Task<List<DistribucionProveedorDto>> GetProductosPorProveedorAsync();
    Task<List<ProductoResponseDto>> GetTop5MasValiososAsync();
    Task<List<ProductoResponseDto>> GetTop5MasStockAsync();
    Task<List<ProductoResponseDto>> GetTop5MenosStockAsync();
    Task<decimal> GetPrecioPromedioAsync();
    Task<decimal> GetPrecioMasAltoAsync();
    Task<decimal> GetPrecioMasBajoAsync();
    Task<int> GetProductosUltimos30DiasAsync();
    Task<PagedResponse<ProductoResponseDto>> GetAllPaginated(int page, int pageSize);
}
