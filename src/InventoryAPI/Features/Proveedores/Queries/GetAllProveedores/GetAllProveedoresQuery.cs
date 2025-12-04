using InventoryAPI.Dtos.ProveedorDtos;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Queries.GetAllProveedores;

public record GetAllProveedoresQuery() : IRequest<IEnumerable<ProveedorResponseDto>>;