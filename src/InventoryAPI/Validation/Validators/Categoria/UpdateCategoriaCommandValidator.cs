using FluentValidation;
using InventoryAPI.Features.Categorias.Commands.UpdateCategoria;

namespace InventoryAPI.Validation.Validators.Categoria;

public class UpdateCategoriaCommandValidator : AbstractValidator<UpdateCategoriaCommand>
{
    public UpdateCategoriaCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .MaximumLength(200).WithMessage("El nombre no puede superar 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Nombre));

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede superar 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));
    }
}