using FluentValidation;
using GestionIntegral.Application.DTOs.Domicilios;

namespace GestionIntegral.Application.Validators;

public class CrearDomicilioRequestValidator : AbstractValidator<CrearDomicilioRequestDto>
{
    public CrearDomicilioRequestValidator()
    {
        RuleFor(x => x.TipoReferencia)
            .NotEmpty()
            .Must(t => t is "C" or "P")
            .WithMessage("El tipo de referencia debe ser C o P.");

        RuleFor(x => x.IdReferencia).GreaterThan(0);

        RuleFor(x => x.Domicilio).NotEmpty().MaximumLength(250);
        RuleFor(x => x.Ciudad).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Localidad).MaximumLength(100);
        RuleFor(x => x.IdProvincia).GreaterThan(0);
        RuleFor(x => x.CodigoPostal).MaximumLength(20);
    }
}
