using FluentValidation;
using GestionIntegral.Application.DTOs.Productos;

namespace GestionIntegral.Application.Validators;

public class CrearProductoRequestValidator : AbstractValidator<CrearProductoRequestDto>
{
    public CrearProductoRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(200);
        RuleFor(x => x.IdCategoria).GreaterThan(0);
        RuleFor(x => x.CodigoBarras).MaximumLength(50);
        RuleFor(x => x.Observaciones).MaximumLength(500);
        RuleFor(x => x.Costo).GreaterThanOrEqualTo(0).When(x => x.Costo.HasValue);
        RuleFor(x => x.StockMinimo).GreaterThanOrEqualTo(0).When(x => x.StockMinimo.HasValue);
        RuleFor(x => x.StockMaximo).GreaterThanOrEqualTo(0).When(x => x.StockMaximo.HasValue);
    }
}
