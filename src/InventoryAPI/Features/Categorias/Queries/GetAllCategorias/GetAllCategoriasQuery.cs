using InventoryAPI.Dtos.CategoriaDtos;
using MediatR;

namespace InventoryAPI.Features.Categorias.Queries.GetAllCategorias;

public record GetAllCategoriasQuery() : IRequest<IEnumerable<CategoriaResponseDto>>;