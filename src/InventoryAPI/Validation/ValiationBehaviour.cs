using FluentValidation;
using MediatR;

namespace InventoryAPI.Validation;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Si no hay validators para este request, continuar
        if (!_validators.Any())
        {
            return await next();
        }

        // Crear contexto de validación
        var context = new ValidationContext<TRequest>(request);

        // Ejecutar todas las validaciones en paralelo
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        // Obtener todos los errores
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        // Si hay errores, lanzar excepción
        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        // Si no hay errores, continuar con el siguiente behavior/handler
        return await next();
    }
}