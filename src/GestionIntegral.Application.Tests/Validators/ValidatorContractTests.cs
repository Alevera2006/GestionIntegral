using System.Reflection;
using FluentAssertions;
using FluentValidation;

namespace GestionIntegral.Application.Tests.Validators;

public class ValidatorContractTests
{
    private static readonly Assembly ApplicationAssembly = typeof(GestionIntegral.Application.DTOs.BaseResponseDto<>).Assembly;

    public static IEnumerable<object[]> ValidatorTypes()
    {
        var validators = ApplicationAssembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } &&
                        t.Namespace == "GestionIntegral.Application.Validators" &&
                        typeof(IValidator).IsAssignableFrom(t))
            .OrderBy(t => t.FullName);

        foreach (var validator in validators)
        {
            yield return [validator];
        }
    }

    [Theory]
    [MemberData(nameof(ValidatorTypes))]
    public void Validator_DebePoderInstanciarse(Type validatorType)
    {
        var instance = Activator.CreateInstance(validatorType);

        instance.Should().NotBeNull($"{validatorType.FullName} debe tener constructor público sin parámetros");
    }

    [Theory]
    [MemberData(nameof(ValidatorTypes))]
    public void Validator_ConModeloDefault_DebeRetornarErrores(Type validatorType)
    {
        var validator = (IValidator)Activator.CreateInstance(validatorType)!;
        var modelType = ObtenerTipoModelo(validatorType);
        modelType.Should().NotBeNull($"no se pudo inferir el modelo validado por {validatorType.Name}");

        var modelInstance = Activator.CreateInstance(modelType!);
        modelInstance.Should().NotBeNull($"{modelType!.Name} debe poder instanciarse para probar {validatorType.Name}");

        var context = new ValidationContext<object>(modelInstance!);
        var result = validator.Validate(context);

        result.IsValid.Should().BeFalse($"{validatorType.Name} debe invalidar una request con valores por defecto");
        result.Errors.Should().NotBeEmpty();
    }

    private static Type? ObtenerTipoModelo(Type validatorType)
    {
        var current = validatorType;
        while (current.BaseType is not null)
        {
            var baseType = current.BaseType;
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
            {
                return baseType.GetGenericArguments()[0];
            }

            current = baseType;
        }

        return null;
    }
}