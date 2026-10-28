using FluentValidation;
using GestionIntegral.Application.DTOs.ListaPrecioDetalles;

namespace GestionIntegral.Application.Validators;

public class CrearListaPrecioDetalleRequestValidator : AbstractValidator<CrearListaPrecioDetalleRequestDto>
{
    public CrearListaPrecioDetalleRequestValidator()
    {
        RuleFor(x => x.IdListaPrecio).GreaterThan(0);
        RuleFor(x => x.IdProducto).GreaterThan(0);
        RuleFor(x => x.Precio).GreaterThanOrEqualTo(0);
    }
}
