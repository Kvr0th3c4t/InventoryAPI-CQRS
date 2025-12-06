using InventoryAPI.Dtos.Pagination;
using InventoryAPI.Dtos.StatsDtos.ProveedoresStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProveedorStats.Queries.GetValorInventarioPorProveedor;

public class GetValorInventarioPorProveedorQueryHandler : IRequestHandler<GetValorInventarioPorProveedorQuery, PagedResponse<DistribucionValorProveedorDto>>
{
    private readonly IProveedorRepository _proveedorRepository;
    public GetValorInventarioPorProveedorQueryHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<PagedResponse<DistribucionValorProveedorDto>> Handle(GetValorInventarioPorProveedorQuery request, CancellationToken cancellationToken)
    {

        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;

        return await _proveedorRepository.GetValorInventarioPorProveedorAsync(pageNumber, pageSize);
    }
}