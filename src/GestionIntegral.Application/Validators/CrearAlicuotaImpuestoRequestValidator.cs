using FluentValidation;
using GestionIntegral.Application.DTOs.AlicuotasImpuesto;

namespace GestionIntegral.Application.Validators;

public class CrearAlicuotaImpuestoRequestValidator : AbstractValidator<CrearAlicuotaImpuestoRequestDto>
{
    public CrearAlicuotaImpuestoRequestValidator()
    {
        RuleFor(x => x.IdTipoImpuesto).GreaterThan(0);
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Porcentaje).GreaterThanOrEqualTo(0);
    }
}
