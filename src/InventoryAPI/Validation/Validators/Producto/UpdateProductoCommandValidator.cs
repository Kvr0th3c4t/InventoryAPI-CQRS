using FluentValidation;
using InventoryAPI.Features.Productos.Commands.UpdateProducto;

namespace InventoryAPI.Validation.Validators.Producto;

public class UpdateProductoCommandValidator : AbstractValidator<UpdateProductoCommand>
{
    public UpdateProductoCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .MaximumLength(200).WithMessage("El nombre no puede superar 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Nombre));

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede superar 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));

        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo")
            .When(x => x.Precio.HasValue);

        RuleFor(x => x.StockActual)
            .GreaterThanOrEqualTo(0).WithMessage("El stock actual no puede ser negativo")
            .When(x => x.StockActual.HasValue);

        RuleFor(x => x.StockMinimo)
            .GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo")
            .When(x => x.StockMinimo.HasValue);

        RuleFor(x => x.CategoriaId)
            .GreaterThan(0).WithMessage("Debe especificar una categoría válida")
            .When(x => x.CategoriaId.HasValue);

        RuleFor(x => x.ProveedorId)
            .GreaterThan(0).WithMessage("Debe especificar un proveedor válido")
            .When(x => x.ProveedorId.HasValue);
    }
}