using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetPrecioMasBajo;

public record GetPrecioMasBajoQuery() : IRequest<decimal>;