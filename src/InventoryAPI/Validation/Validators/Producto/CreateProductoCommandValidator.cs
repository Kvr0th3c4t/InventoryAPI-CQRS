using FluentValidation;
using InventoryAPI.Features.Productos.Commands.CreateProducto;

namespace InventoryAPI.Validation.Validators.Producto;

public class CreateProductoCommandValidator : AbstractValidator<CreateProductoCommand>
{
    public CreateProductoCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(200).WithMessage("El nombre no puede superar 200 caracteres");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede superar 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));

        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo");

        RuleFor(x => x.StockActual)
            .GreaterThanOrEqualTo(0).WithMessage("El stock actual no puede ser negativo");

        RuleFor(x => x.StockMinimo)
            .GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo");

        RuleFor(x => x.CategoriaId)
            .GreaterThan(0).WithMessage("Debe especificar una categoría válida");

        RuleFor(x => x.ProveedorId)
            .GreaterThan(0).WithMessage("Debe especificar un proveedor válido")
            .When(x => x.ProveedorId.HasValue);
    }
}