using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Repositories;
using InventoryAPI.Dtos.Pagination;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Queries.GetAllProveedores;

public class GetAllProveedoresQueryHandler
    : IRequestHandler<GetAllProveedoresQuery, PagedResponse<ProveedorResponseDto>>
{
    private readonly IProveedorRepository _proveedorRepository;

    public GetAllProveedoresQueryHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<PagedResponse<ProveedorResponseDto>> Handle(
        GetAllProveedoresQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;

        return await _proveedorRepository.GetAllPaginated(pageNumber, pageSize);
    }
}