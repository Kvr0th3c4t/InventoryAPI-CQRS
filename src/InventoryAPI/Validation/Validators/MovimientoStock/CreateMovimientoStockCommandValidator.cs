using FluentValidation;
using InventoryAPI.Enums;
using InventoryAPI.Features.MovimientosStock.Commands.CreateMovimiento;

namespace InventoryAPI.Validation.Validators.MovimientoStock;

public class CreateMovimientoStockCommandValidator : AbstractValidator<CreateMovimientoStockCommand>
{
    public CreateMovimientoStockCommandValidator()
    {
        RuleFor(x => x.ProductoId)
            .GreaterThan(0).WithMessage("Debe especificar un producto válido");

        RuleFor(x => x.Cantidad)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0");

        RuleFor(x => x.Tipo)
            .IsInEnum().WithMessage("El tipo de movimiento no es válido");

        RuleFor(x => x.Razon)
            .MaximumLength(500).WithMessage("La razón no puede superar 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Razon));

        // Validación en 2 pasos
        RuleFor(x => x.ProveedorId)
            .NotNull().WithMessage("Para movimientos de entrada es obligatorio el ProveedorId")
            .When(x => x.Tipo == TipoMovimiento.Entrada);

        RuleFor(x => x.ProveedorId)
            .GreaterThan(0).WithMessage("Debe especificar un proveedor válido")
            .When(x => x.ProveedorId.HasValue);
    }
}