using FluentValidation;
using GestionIntegral.Application.DTOs.Provincias;

namespace GestionIntegral.Application.Validators;

public class CrearProvinciaRequestValidator : AbstractValidator<CrearProvinciaRequestDto>
{
    public CrearProvinciaRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CodigoAFIP).MaximumLength(10);
    }
}
