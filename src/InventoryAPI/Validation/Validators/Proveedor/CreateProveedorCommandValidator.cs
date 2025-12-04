using FluentValidation;
using InventoryAPI.Features.Proveedores.Commands.CreateProveedor;

namespace InventoryAPI.Validation.Validators.Proveedor;

public class CreateProveedorCommandValidator : AbstractValidator<CreateProveedorCommand>
{
    public CreateProveedorCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(200).WithMessage("El nombre no puede superar 200 caracteres");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("El email no tiene un formato válido")
            .MaximumLength(100).WithMessage("El email no puede superar 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Telefono)
            .MaximumLength(20).WithMessage("El teléfono no puede superar 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Telefono));
    }
}