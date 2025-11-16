using InventoryAPI.Dtos.CategoriaDtos;
using MediatR;

namespace InventoryAPI.Features.Categorias.Commands.CreateCategoria;

public record CreateCategoriaCommand(
    string Nombre,
    string Descripcion
) : IRequest<CategoriaResponseDto>;