using MediatR;

namespace InventoryAPI.Features.Stats.ProveedorStats.Queries.GetTotalProveedores;

public record GetTotalProveedoresQuery : IRequest<int>;