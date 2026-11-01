using System.Reflection;
using FluentAssertions;

namespace GestionIntegral.Application.Tests.DTOs;

public class DtoContractTests
{
    private static readonly Assembly ApplicationAssembly = typeof(GestionIntegral.Application.DTOs.BaseResponseDto<>).Assembly;

    public static IEnumerable<object[]> DtoTypes()
    {
        var dtoTypes = ApplicationAssembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } &&
                        t.Namespace is not null &&
                        t.Namespace.StartsWith("GestionIntegral.Application.DTOs", StringComparison.Ordinal))
            .OrderBy(t => t.FullName);

        foreach (var dtoType in dtoTypes)
        {
            yield return [dtoType];
        }
    }

    [Theory]
    [MemberData(nameof(DtoTypes))]
    public void Dto_DebePoderInstanciarse(Type dtoType)
    {
        var typeToCreate = dtoType.IsGenericTypeDefinition
            ? dtoType.MakeGenericType(typeof(string))
            : dtoType;

        var instance = Activator.CreateInstance(typeToCreate);

        instance.Should().NotBeNull($"{typeToCreate.FullName} debe tener constructor por defecto público");
    }

    [Theory]
    [MemberData(nameof(DtoTypes))]
    public void Dto_PropiedadesSeteables_DebenAceptarValores(Type dtoType)
    {
        var typeToCreate = dtoType.IsGenericTypeDefinition
            ? dtoType.MakeGenericType(typeof(string))
            : dtoType;

        var instance = Activator.CreateInstance(typeToCreate);
        instance.Should().NotBeNull();

        var propiedades = typeToCreate.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite && p.GetIndexParameters().Length == 0);

        foreach (var propiedad in propiedades)
        {
            var valor = CrearValorPara(propiedad.PropertyType);
            if (valor is null)
            {
                continue;
            }

            propiedad.SetValue(instance, valor);
            var valorActual = propiedad.GetValue(instance);
            valorActual.Should().Be(valor, $"la propiedad {propiedad.Name} de {typeToCreate.Name} debe poder actualizarse");
        }
    }

    private static object? CrearValorPara(Type propertyType)
    {
        var tipo = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (tipo == typeof(string)) return "valor-prueba";
        if (tipo == typeof(int)) return 123;
        if (tipo == typeof(long)) return 123L;
        if (tipo == typeof(bool)) return true;
        if (tipo == typeof(DateTime)) return new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        if (tipo == typeof(decimal)) return 10m;
        if (tipo == typeof(double)) return 10d;
        if (tipo == typeof(float)) return 10f;
        if (tipo.IsEnum) return Enum.GetValues(tipo).GetValue(0);

        if (tipo.IsArray)
        {
            return Array.CreateInstance(tipo.GetElementType()!, 0);
        }

        if (tipo.IsClass)
        {
            return Activator.CreateInstance(tipo);
        }

        return null;
    }
}