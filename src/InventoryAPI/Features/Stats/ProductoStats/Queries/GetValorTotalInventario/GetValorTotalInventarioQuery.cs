using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetValorTotalInventario;

public record GetValorTotalInventarioQuery() : IRequest<decimal>;