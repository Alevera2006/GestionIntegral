using FluentValidation;
using GestionIntegral.Application.DTOs.TiposMovimientoCaja;

namespace GestionIntegral.Application.Validators;

public class CrearTipoMovimientoCajaRequestValidator : AbstractValidator<CrearTipoMovimientoCajaRequestDto>
{
    public CrearTipoMovimientoCajaRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Signo).Must(s => s == -1 || s == 1).WithMessage("El signo debe ser -1 o 1.");
    }
}
