using FluentValidation;
using GestionIntegral.Application.DTOs.Usuarios;

namespace GestionIntegral.Application.Validators;

public class CrearUsuarioRequestValidator : AbstractValidator<CrearUsuarioRequestDto>
{
    public CrearUsuarioRequestValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Apellido).NotEmpty().MaximumLength(100);
        RuleFor(x => x.UsuarioNombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Clave).NotEmpty().MinimumLength(6);
        RuleFor(x => x.IdTipoUsuario).GreaterThan(0);
    }
}
