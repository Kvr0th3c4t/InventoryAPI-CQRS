using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetPrecioMasAlto;

public record GetPrecioMasAltoQuery() : IRequest<decimal>;