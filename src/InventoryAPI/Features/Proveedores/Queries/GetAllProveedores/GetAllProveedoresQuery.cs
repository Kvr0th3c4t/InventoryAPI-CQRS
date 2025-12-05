using InventoryAPI.Dtos.ProveedorDtos;
using MediatR;
using InventoryAPI.Dtos.Pagination;

namespace InventoryAPI.Features.Proveedores.Queries.GetAllProveedores;

public record GetAllProveedoresQuery(int Page = 1, int PageSize = 10)
    : IRequest<PagedResponse<ProveedorResponseDto>>;