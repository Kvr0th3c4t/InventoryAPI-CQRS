using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosPorCategoria;

public record GetProductosPorCategoriaQuery() : IRequest<IEnumerable<DistribucionCategoriaDto>>;