using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosStockBajo;

public record GetProductosStockBajoQuery() : IRequest<int>;