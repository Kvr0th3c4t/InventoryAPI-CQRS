using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosSinStock;

public record GetProductosSinStockQuery() : IRequest<int>;