using FluentValidation;
using GestionIntegral.Application.DTOs.EstadosPedido;

namespace GestionIntegral.Application.Validators;

public class CrearEstadoPedidoRequestValidator : AbstractValidator<CrearEstadoPedidoRequestDto>
{
    public CrearEstadoPedidoRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
    }
}
