using FluentValidation;
using GestionIntegral.Application.DTOs.Proveedores;

namespace GestionIntegral.Application.Validators;

public class CrearProveedorRequestValidator : AbstractValidator<CrearProveedorRequestDto>
{
    public CrearProveedorRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.RazonSocial).NotEmpty().MaximumLength(150);
        RuleFor(x => x.NombreFantasia).MaximumLength(150);
        RuleFor(x => x.CUIT).MaximumLength(13);
        RuleFor(x => x.Email).MaximumLength(150).EmailAddress();
        RuleFor(x => x.Telefono).MaximumLength(50);
        RuleFor(x => x.Observaciones).MaximumLength(500);
    }
}
