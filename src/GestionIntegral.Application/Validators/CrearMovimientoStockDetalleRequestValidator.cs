using FluentValidation;
using GestionIntegral.Application.DTOs.MovimientosStock;

namespace GestionIntegral.Application.Validators;

public class CrearMovimientoStockDetalleRequestValidator : AbstractValidator<CrearMovimientoStockDetalleRequestDto>
{
    public CrearMovimientoStockDetalleRequestValidator()
    {
        RuleFor(x => x.IdMovimientoStock).GreaterThan(0);
        RuleFor(x => x.IdProducto).GreaterThan(0);
        RuleFor(x => x.Cantidad).GreaterThan(0);
    }
}
