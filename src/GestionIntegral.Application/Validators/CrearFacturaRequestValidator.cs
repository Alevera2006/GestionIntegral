using FluentValidation;
using GestionIntegral.Application.DTOs.Facturacion;

namespace GestionIntegral.Application.Validators;

public class CrearFacturaRequestValidator : AbstractValidator<FacturaRequestDto>
{
    public CrearFacturaRequestValidator()
    {
        RuleFor(x => x.IdTipoComprobante).GreaterThan(0).WithMessage("El tipo de comprobante es obligatorio.");
        RuleFor(x => x.NumeroComprobante).NotEmpty().MaximumLength(20).WithMessage("El número de comprobante es obligatorio.");
        RuleFor(x => x.IdCondicionPago).GreaterThan(0).WithMessage("La condición de pago es obligatoria.");
        RuleFor(x => x.Fecha).NotEmpty().WithMessage("La fecha es obligatoria.");
        RuleFor(x => x.Detalles).NotEmpty().WithMessage("La factura debe tener al menos un ítem.");
        RuleForEach(x => x.Detalles).ChildRules(detalle =>
        {
            detalle.RuleFor(d => d.IdProducto).GreaterThan(0);
            detalle.RuleFor(d => d.Cantidad).GreaterThan(0);
            detalle.RuleFor(d => d.PrecioUnitario).GreaterThanOrEqualTo(0);
        });
    }
}
