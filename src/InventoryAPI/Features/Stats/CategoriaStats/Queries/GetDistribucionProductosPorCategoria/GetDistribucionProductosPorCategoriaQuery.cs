using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using MediatR;

namespace InventoryAPI.Features.Stats.CategoriaStats.Queries.GetDistribucionProductosPorCategoria;

public record GetDistribucionProductosPorCategoriaQuery : IRequest<IEnumerable<DistribucionCategoriaDto>>
{

}