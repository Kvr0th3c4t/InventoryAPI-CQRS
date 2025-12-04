using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosUltimos30Dias;

public record GetProductosUltimos30DiasQuery() : IRequest<int>;