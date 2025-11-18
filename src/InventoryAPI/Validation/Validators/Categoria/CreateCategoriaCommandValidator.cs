using FluentValidation;
using InventoryAPI.Features.Categorias.Commands.CreateCategoria;

namespace InventoryAPI.Validation.Validators.Categoria;

public class CreateCategoriaCommandValidator : AbstractValidator<CreateCategoriaCommand>
{
    public CreateCategoriaCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(200).WithMessage("El nombre no puede superar 200 caracteres");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede superar 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));
    }
}