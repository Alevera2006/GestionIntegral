using FluentValidation;
using GestionIntegral.Application.DTOs.ClienteContactos;

namespace GestionIntegral.Application.Validators;

public class CrearClienteContactoRequestValidator : AbstractValidator<CrearClienteContactoRequestDto>
{
    public CrearClienteContactoRequestValidator()
    {
        RuleFor(x => x.IdCliente).GreaterThan(0);
        RuleFor(x => x.IdContacto).GreaterThan(0);
    }
}
