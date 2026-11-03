using FluentValidation;
using GestionIntegral.Application.DTOs.MovimientosStock;

namespace GestionIntegral.Application.Validators;

public class CrearMovimientoStockConDetallesRequestValidator : AbstractValidator<CrearMovimientoStockConDetallesRequestDto>
{
    public CrearMovimientoStockConDetallesRequestValidator()
    {
        RuleFor(x => x.IdDeposito).GreaterThan(0);
        RuleFor(x => x.IdTipoMovimientoStock).GreaterThan(0);
        RuleFor(x => x.IdUsuarioResponsable).GreaterThan(0);
        RuleFor(x => x.Referencia).MaximumLength(100);
        RuleFor(x => x.Observaciones).MaximumLength(500);
        RuleFor(x => x.Detalles).NotEmpty();
        RuleForEach(x => x.Detalles).ChildRules(detalle =>
        {
            detalle.RuleFor(x => x.IdProducto).GreaterThan(0);
            detalle.RuleFor(x => x.Cantidad).GreaterThan(0);
        });
    }
}
