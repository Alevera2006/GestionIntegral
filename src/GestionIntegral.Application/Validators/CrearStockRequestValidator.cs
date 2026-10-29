using FluentValidation;
using GestionIntegral.Application.DTOs.Stock;

namespace GestionIntegral.Application.Validators;

public class CrearStockRequestValidator : AbstractValidator<CrearStockRequestDto>
{
    public CrearStockRequestValidator()
    {
        RuleFor(x => x.IdDeposito).GreaterThan(0);
        RuleFor(x => x.IdProducto).GreaterThan(0);
    }
}
