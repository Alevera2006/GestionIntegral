using FluentValidation;
using GestionIntegral.Application.DTOs.ListasPrecios;

namespace GestionIntegral.Application.Validators;

public class CrearListaPrecioRequestValidator : AbstractValidator<CrearListaPrecioRequestDto>
{
    public CrearListaPrecioRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
    }
}
