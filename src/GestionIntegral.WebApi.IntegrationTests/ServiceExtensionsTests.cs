using FluentAssertions;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace GestionIntegral.WebApi.IntegrationTests;

public class ServiceExtensionsTests
{
    [Fact]
    public void AddApplicationServices_RegistrarServiciosPrincipales()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();

        var result = services.AddApplicationServices(configuration);

        result.Should().BeSameAs(services);
        services.Should().Contain(x => x.ServiceType == typeof(IAuthService));
        services.Should().Contain(x => x.ServiceType == typeof(IClienteService));
        services.Should().Contain(x => x.ServiceType == typeof(IProveedorService));
        services.Should().Contain(x => x.ServiceType == typeof(IProductoService));
        services.Should().Contain(x => x.ServiceType == typeof(IUsuarioService));
    }

    [Fact]
    public void AddJwtAuthentication_SinSecret_LanzaExcepcion()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "issuer-test",
                ["Jwt:Audience"] = "audience-test"
            })
            .Build();

        var act = () => services.AddJwtAuthentication(configuration);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddJwtAuthentication_ConConfiguracionValida_RegistraAuthYOptions()
    {
        var services = new ServiceCollection();
        services.AddLogging();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "issuer-test",
                ["Jwt:Audience"] = "audience-test",
                ["Jwt:Secret"] = "super-secret-key-with-enough-length-123456"
            })
            .Build();

        var result = services.AddJwtAuthentication(configuration);
        var provider = services.BuildServiceProvider();

        result.Should().BeSameAs(services);
        provider.GetService<IAuthenticationService>().Should().NotBeNull();
        provider.GetService<IAuthorizationService>().Should().NotBeNull();

        var jwtOptions = provider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>()
            .Get(JwtBearerDefaults.AuthenticationScheme);

        jwtOptions.TokenValidationParameters.ValidIssuer.Should().Be("issuer-test");
        jwtOptions.TokenValidationParameters.ValidAudience.Should().Be("audience-test");
        jwtOptions.TokenValidationParameters.ClockSkew.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public void AddCustomSwagger_RegistraSwaggerProvider()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddRouting();
        services.AddEndpointsApiExplorer();
        services.AddSingleton<IWebHostEnvironment>(new TestWebHostEnvironment());
        services.AddSingleton<Microsoft.Extensions.Hosting.IHostEnvironment>(sp => sp.GetRequiredService<IWebHostEnvironment>());
        services.AddSingleton<EndpointDataSource>(new DefaultEndpointDataSource(Array.Empty<Endpoint>()));

        var result = services.AddCustomSwagger();
        var provider = services.BuildServiceProvider();

        result.Should().BeSameAs(services);
        provider.GetService<ISwaggerProvider>().Should().NotBeNull();
    }

    private sealed class TestWebHostEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "GestionIntegral.WebApi.IntegrationTests";
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string WebRootPath { get; set; } = string.Empty;
        public string EnvironmentName { get; set; } = "Testing";
        public string ContentRootPath { get; set; } = string.Empty;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
