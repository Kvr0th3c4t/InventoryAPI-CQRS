
using MediatR;

namespace InventoryAPI.Features.Stats.CategoriaStats.Queries.GetTotalCategorias;

public record GetTotalCategoriasQuery : IRequest<int>
{

}