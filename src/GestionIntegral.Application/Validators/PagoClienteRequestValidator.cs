using FluentValidation;
using GestionIntegral.Application.DTOs.CuentasCorrientes;

namespace GestionIntegral.Application.Validators;

public class PagoClienteRequestValidator : AbstractValidator<PagoClienteRequestDto>
{
    public PagoClienteRequestValidator()
    {
        RuleFor(x => x.IdCliente).GreaterThan(0).WithMessage("El cliente es obligatorio.");
        RuleFor(x => x.IdMedioPago).GreaterThan(0).WithMessage("El medio de pago es obligatorio.");
        RuleFor(x => x.Monto).GreaterThan(0).WithMessage("El monto debe ser mayor a cero.");
        RuleFor(x => x.Fecha).NotEmpty().WithMessage("La fecha es obligatoria.");
    }
}
