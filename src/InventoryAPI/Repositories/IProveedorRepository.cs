using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Dtos.StatsDtos.ProveedoresStatsDto;
using InventoryAPI.Models;
using InventoryAPI.Dtos.Pagination;

namespace InventoryAPI.Repositories;

public interface IProveedorRepository : IGenericRepository<Proveedor>
{

    Task<int> GetTotalProveedoresAsync();
    Task<PagedResponse<DistribucionProveedorDto>> GetProductosPorProveedorAsync(int pageNumber, int pageSize);
    Task<ProveedorResponseDto?> GetProveedorMasActivoAsync();
    Task<PagedResponse<DistribucionValorProveedorDto>> GetValorInventarioPorProveedorAsync(int pageNumber, int pageSize);
    Task<PagedResponse<ProveedorResponseDto>> GetAllPaginated(int page, int pageSize);
}