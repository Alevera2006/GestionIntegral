using FluentValidation;
using GestionIntegral.Application.DTOs.Emails;

namespace GestionIntegral.Application.Validators;

public class CrearEmailRequestValidator : AbstractValidator<CrearEmailRequestDto>
{
    public CrearEmailRequestValidator()
    {
        RuleFor(x => x.TipoReferencia)
            .NotEmpty()
            .Must(t => t is "C" or "P")
            .WithMessage("El tipo de referencia debe ser C o P.");

        RuleFor(x => x.IdReferencia).GreaterThan(0);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(254).EmailAddress();
    }
}
