using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetPrecioPromedio;

public record GetPrecioPromedioQuery() : IRequest<decimal>;