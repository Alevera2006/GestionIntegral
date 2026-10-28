using FluentValidation;
using GestionIntegral.Application.DTOs.CondicionesPago;

namespace GestionIntegral.Application.Validators;

public class CrearCondicionPagoRequestValidator : AbstractValidator<CrearCondicionPagoRequestDto>
{
    public CrearCondicionPagoRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dias).GreaterThanOrEqualTo(0);
    }
}
