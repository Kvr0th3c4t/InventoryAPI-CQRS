using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Models;

namespace InventoryAPI.Repositories;

public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    Task<int> GetTotalCategoriasAsync();
    Task<List<DistribucionCategoriaDto>> GetDistribucionProductosPorCategoriaAsync();
    Task<CategoriaResponseDto?> GetCategoriaConMasProductosAsync();
    Task<CategoriaResponseDto?> GetCategoriaConMayorValorAsync();

}