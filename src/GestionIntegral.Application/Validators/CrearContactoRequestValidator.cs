using FluentValidation;
using GestionIntegral.Application.DTOs.Contactos;

namespace GestionIntegral.Application.Validators;

public class CrearContactoRequestValidator : AbstractValidator<CrearContactoRequestDto>
{
    public CrearContactoRequestValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Apellido).MaximumLength(100);
        RuleFor(x => x.Cargo).MaximumLength(100);
        RuleFor(x => x.Telefono).MaximumLength(50);
        RuleFor(x => x.Celular).MaximumLength(50);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(150);
    }
}
