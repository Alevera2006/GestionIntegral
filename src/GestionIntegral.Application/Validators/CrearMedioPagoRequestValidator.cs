using FluentValidation;
using GestionIntegral.Application.DTOs.MediosPago;

namespace GestionIntegral.Application.Validators;

public class CrearMedioPagoRequestValidator : AbstractValidator<CrearMedioPagoRequestDto>
{
    public CrearMedioPagoRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
    }
}
