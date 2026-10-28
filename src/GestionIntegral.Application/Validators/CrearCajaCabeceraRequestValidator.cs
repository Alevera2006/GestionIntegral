using FluentValidation;
using GestionIntegral.Application.DTOs.Cajas;

namespace GestionIntegral.Application.Validators;

public class CrearCajaCabeceraRequestValidator : AbstractValidator<CrearCajaCabeceraRequestDto>
{
    public CrearCajaCabeceraRequestValidator()
    {
        RuleFor(x => x.FechaCaja).NotEmpty();
        RuleFor(x => x.IdUsuarioResponsable).GreaterThan(0);
        RuleFor(x => x.IdEstadoCaja).GreaterThan(0);
        RuleFor(x => x.Observaciones).MaximumLength(500);
    }
}
