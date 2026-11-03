using FluentValidation;
using GestionIntegral.Application.DTOs.PagosProveedores;

namespace GestionIntegral.Application.Validators;

public class PagoProveedorRequestValidator : AbstractValidator<PagoProveedorRequestDto>
{
    public PagoProveedorRequestValidator()
    {
        RuleFor(x => x.IdProveedor).GreaterThan(0).WithMessage("El proveedor es obligatorio.");
        RuleFor(x => x.IdMedioPago).GreaterThan(0).WithMessage("El medio de pago es obligatorio.");
        RuleFor(x => x.Monto).GreaterThan(0).WithMessage("El monto debe ser mayor a cero.");
        RuleFor(x => x.Fecha).NotEmpty().WithMessage("La fecha es obligatoria.");
        RuleFor(x => x.Concepto).NotEmpty().MaximumLength(250).WithMessage("El concepto es obligatorio.");
    }
}
