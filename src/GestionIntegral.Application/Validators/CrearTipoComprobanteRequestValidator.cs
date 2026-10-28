using FluentValidation;
using GestionIntegral.Application.DTOs.TiposComprobante;

namespace GestionIntegral.Application.Validators;

public class CrearTipoComprobanteRequestValidator : AbstractValidator<CrearTipoComprobanteRequestDto>
{
    public CrearTipoComprobanteRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CodigoAFIP).MaximumLength(10);
    }
}
