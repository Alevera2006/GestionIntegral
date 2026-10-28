using FluentValidation;
using GestionIntegral.Application.DTOs.Categorias;

namespace GestionIntegral.Application.Validators;

public class CrearCategoriaRequestValidator : AbstractValidator<CrearCategoriaRequestDto>
{
    public CrearCategoriaRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
    }
}
