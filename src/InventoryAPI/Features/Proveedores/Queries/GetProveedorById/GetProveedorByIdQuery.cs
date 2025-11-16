using InventoryAPI.Dtos.ProveedorDtos;
using MediatR;

namespace InventoryAPI.Features.Proveedores.Queries.GetProveedorById;

public record GetProveedorByIdQuery(int Id) : IRequest<ProveedorResponseDto>;