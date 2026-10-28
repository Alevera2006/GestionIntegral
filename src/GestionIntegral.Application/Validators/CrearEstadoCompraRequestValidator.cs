using FluentValidation;
using GestionIntegral.Application.DTOs.EstadosCompra;

namespace GestionIntegral.Application.Validators;

public class CrearEstadoCompraRequestValidator : AbstractValidator<CrearEstadoCompraRequestDto>
{
    public CrearEstadoCompraRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
    }
}
