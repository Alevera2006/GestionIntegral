using FluentValidation;
using GestionIntegral.Application.DTOs.TiposImpuesto;

namespace GestionIntegral.Application.Validators;

public class CrearTipoImpuestoRequestValidator : AbstractValidator<CrearTipoImpuestoRequestDto>
{
    public CrearTipoImpuestoRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
    }
}
