using InventoryAPI.Dtos.CategoriaDtos;
using MediatR;

namespace InventoryAPI.Features.Stats.CategoriaStats.Queries.GetCategoriaConMayorValor;

public record GetCategoriaConMayorValorQuery : IRequest<CategoriaResponseDto?>
{

}