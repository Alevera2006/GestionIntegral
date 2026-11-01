using FluentAssertions;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using GestionIntegral.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.WebApi.IntegrationTests.Repositories;

public class UnitOfWorkRepositoriesTests
{
    [Fact]
    public async Task UnitOfWork_DebeExponerTodosLosRepositoriosConfigurados()
    {
        var options = new DbContextOptionsBuilder<GestionIntegralDbContext>()
            .UseInMemoryDatabase($"uow-repo-test-{Guid.NewGuid()}")
            .Options;

        await using var context = new GestionIntegralDbContext(options);
        using var unitOfWork = new UnitOfWork(context);

        var repositoryProperties = typeof(IUnitOfWork)
            .GetProperties()
            .Where(p => p.PropertyType.IsInterface && p.PropertyType.Name.EndsWith("Repository"));

        foreach (var property in repositoryProperties)
        {
            var value = property.GetValue(unitOfWork);
            value.Should().NotBeNull($"el repositorio '{property.Name}' debe estar inicializado en UnitOfWork");
        }
    }

    [Fact]
    public async Task SaveChangesAsync_SinCambios_RetornaCero()
    {
        var options = new DbContextOptionsBuilder<GestionIntegralDbContext>()
            .UseInMemoryDatabase($"uow-savechanges-test-{Guid.NewGuid()}")
            .Options;

        await using var context = new GestionIntegralDbContext(options);
        using var unitOfWork = new UnitOfWork(context);

        var result = await unitOfWork.SaveChangesAsync();

        result.Should().Be(0);
    }
}
