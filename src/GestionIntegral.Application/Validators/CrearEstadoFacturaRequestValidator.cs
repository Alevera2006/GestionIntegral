using FluentValidation;
using GestionIntegral.Application.DTOs.EstadosFactura;

namespace GestionIntegral.Application.Validators;

public class CrearEstadoFacturaRequestValidator : AbstractValidator<CrearEstadoFacturaRequestDto>
{
    public CrearEstadoFacturaRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
    }
}
