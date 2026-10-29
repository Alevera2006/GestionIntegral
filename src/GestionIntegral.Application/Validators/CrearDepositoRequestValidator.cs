using FluentValidation;
using GestionIntegral.Application.DTOs.Depositos;

namespace GestionIntegral.Application.Validators;

public class CrearDepositoRequestValidator : AbstractValidator<CrearDepositoRequestDto>
{
    public CrearDepositoRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Ubicacion).MaximumLength(200);
    }
}
