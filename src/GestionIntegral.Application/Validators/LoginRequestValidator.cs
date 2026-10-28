using FluentValidation;
using GestionIntegral.Application.DTOs.Auth;

namespace GestionIntegral.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Usuario).NotEmpty();
        RuleFor(x => x.Clave).NotEmpty();
    }
}
