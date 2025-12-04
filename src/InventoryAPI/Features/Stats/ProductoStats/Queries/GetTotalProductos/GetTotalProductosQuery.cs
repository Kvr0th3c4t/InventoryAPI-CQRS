using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetTotalProductos;

public record GetTotalProductosQuery() : IRequest<int>;