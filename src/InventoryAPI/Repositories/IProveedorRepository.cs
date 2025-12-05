using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Dtos.StatsDtos.ProveedoresStatsDto;
using InventoryAPI.Models;
using InventoryAPI.Dtos.Pagination;

namespace InventoryAPI.Repositories;

public interface IProveedorRepository : IGenericRepository<Proveedor>
{

    Task<int> GetTotalProveedoresAsync();
    Task<List<DistribucionProveedorDto>> GetProductosPorProveedorAsync();
    Task<ProveedorResponseDto?> GetProveedorMasActivoAsync();
    Task<List<DistribucionValorProveedorDto>> GetValorInventarioPorProveedorAsync();
    Task<PagedResponse<ProveedorResponseDto>> GetAllPaginated(int page, int pageSize);
}