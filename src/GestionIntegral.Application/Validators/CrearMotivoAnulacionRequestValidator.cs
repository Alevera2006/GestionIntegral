using FluentValidation;
using GestionIntegral.Application.DTOs.MotivosAnulacion;

namespace GestionIntegral.Application.Validators;

public class CrearMotivoAnulacionRequestValidator : AbstractValidator<CrearMotivoAnulacionRequestDto>
{
    public CrearMotivoAnulacionRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(200);
    }
}
