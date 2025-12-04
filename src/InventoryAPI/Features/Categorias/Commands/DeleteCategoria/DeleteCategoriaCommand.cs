using MediatR;

namespace InventoryAPI.Features.Categorias.Commands.DeleteCategoria;

public record DeleteCategoriaCommand(int Id) : IRequest<bool>;