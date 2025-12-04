using InventoryAPI.Dtos.CategoriaDtos;
using MediatR;

namespace InventoryAPI.Features.Stats.CategoriaStats.Queries.GetCategoriaConMasProductos;

public record GetCategoriaConMasProductosQuery() : IRequest<CategoriaResponseDto?>
{

}