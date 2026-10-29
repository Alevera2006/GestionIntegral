using FluentValidation;
using GestionIntegral.Application.DTOs.MovimientosStock;

namespace GestionIntegral.Application.Validators;

public class CrearMovimientoStockRequestValidator : AbstractValidator<CrearMovimientoStockRequestDto>
{
    public CrearMovimientoStockRequestValidator()
    {
        RuleFor(x => x.IdDeposito).GreaterThan(0);
        RuleFor(x => x.IdTipoMovimientoStock).GreaterThan(0);
        RuleFor(x => x.IdUsuarioResponsable).GreaterThan(0);
        RuleFor(x => x.Referencia).MaximumLength(100);
        RuleFor(x => x.Observaciones).MaximumLength(500);
    }
}
