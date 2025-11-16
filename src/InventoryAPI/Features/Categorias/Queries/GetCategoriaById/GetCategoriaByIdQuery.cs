using InventoryAPI.Dtos.CategoriaDtos;
using MediatR;

namespace InventoryAPI.Features.Categorias.Queries.GetCategoriaById;

public record GetCategoriaByIdQuery(int Id) : IRequest<CategoriaResponseDto>;

