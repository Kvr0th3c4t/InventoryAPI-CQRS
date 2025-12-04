using InventoryAPI.Dtos.ProveedorDtos;
using MediatR;

namespace InventoryAPI.Features.Stats.ProveedorStats.Queries.GetProveedoresMasActivos;

public record GetProveedoresMasActivosQuery : IRequest<ProveedorResponseDto?>;