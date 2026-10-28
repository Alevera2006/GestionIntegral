using FluentValidation;
using GestionIntegral.Application.DTOs.Monedas;

namespace GestionIntegral.Application.Validators;

public class CrearMonedaRequestValidator : AbstractValidator<CrearMonedaRequestDto>
{
    public CrearMonedaRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Simbolo).MaximumLength(10);
        RuleFor(x => x.CantidadDecimales).InclusiveBetween((byte)0, (byte)6);
    }
}
