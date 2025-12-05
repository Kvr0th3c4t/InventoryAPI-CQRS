using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Models;
using InventoryAPI.Dtos.Pagination;

namespace InventoryAPI.Repositories;

public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    Task<int> GetTotalCategoriasAsync();
    Task<List<DistribucionCategoriaDto>> GetDistribucionProductosPorCategoriaAsync();
    Task<CategoriaResponseDto?> GetCategoriaConMasProductosAsync();
    Task<CategoriaResponseDto?> GetCategoriaConMayorValorAsync();
    Task<PagedResponse<CategoriaResponseDto>> GetAllPaginated(int page, int pageSize);

}