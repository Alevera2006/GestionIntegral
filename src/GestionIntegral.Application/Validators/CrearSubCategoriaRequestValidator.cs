using FluentValidation;
using GestionIntegral.Application.DTOs.SubCategorias;

namespace GestionIntegral.Application.Validators;

public class CrearSubCategoriaRequestValidator : AbstractValidator<CrearSubCategoriaRequestDto>
{
    public CrearSubCategoriaRequestValidator()
    {
        RuleFor(x => x.IdCategoria).GreaterThan(0);
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
    }
}
