using FluentValidation;
using GestionIntegral.Application.DTOs.EstadosCaja;

namespace GestionIntegral.Application.Validators;

public class CrearEstadoCajaRequestValidator : AbstractValidator<CrearEstadoCajaRequestDto>
{
    public CrearEstadoCajaRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
    }
}
