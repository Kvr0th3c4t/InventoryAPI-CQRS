using InventoryAPI.Dtos.CategoriaDtos;
using MediatR;

namespace InventoryAPI.Features.Categorias.Commands.UpdateCategoria;

public record UpdateCategoriaCommand(
    int Id,
    string? Nombre,
    string? Descripcion

) : IRequest<CategoriaResponseDto>;