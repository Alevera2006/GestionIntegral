using FluentValidation;
using GestionIntegral.Application.DTOs.Cajas;

namespace GestionIntegral.Application.Validators;

public class CrearCajaDetalleRequestValidator : AbstractValidator<CrearCajaDetalleRequestDto>
{
    public CrearCajaDetalleRequestValidator()
    {
        RuleFor(x => x.IdCaja).GreaterThan(0);
        RuleFor(x => x.IdTipoMovimientoCaja).GreaterThan(0);
        RuleFor(x => x.Concepto).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Referencia).MaximumLength(100);
        RuleFor(x => x.Importe).GreaterThan(0);
    }
}
