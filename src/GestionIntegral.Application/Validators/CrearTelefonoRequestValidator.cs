using FluentValidation;
using GestionIntegral.Application.DTOs.Telefonos;

namespace GestionIntegral.Application.Validators;

public class CrearTelefonoRequestValidator : AbstractValidator<CrearTelefonoRequestDto>
{
    public CrearTelefonoRequestValidator()
    {
        RuleFor(x => x.TipoReferencia)
            .NotEmpty()
            .Must(t => t is "C" or "P")
            .WithMessage("El tipo de referencia debe ser C o P.");

        RuleFor(x => x.IdReferencia).GreaterThan(0);

        RuleFor(x => x.NumeroTelefono).NotEmpty().MaximumLength(50);
        RuleFor(x => x.TipoTelefono)
            .NotEmpty()
            .Must(t => t is "Fijo" or "Celular")
            .WithMessage("El tipo de teléfono debe ser Fijo o Celular.");
    }
}
