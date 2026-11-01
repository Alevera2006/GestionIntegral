using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Auth;
using GestionIntegral.Domain.Common.Exceptions;
using Moq;

namespace GestionIntegral.WebApi.IntegrationTests.Controllers;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public AuthControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ConCredencialesValidas_RetornaOkConToken()
    {
        // Arrange
        _factory.AuthServiceMock.Reset();

        var request = new LoginRequestDto { Usuario = "admin", Clave = "password" };
        var expected = BaseResponseDto<LoginResponseDto>.Ok(new LoginResponseDto
        {
            Token = "jwt-token-test",
            Usuario = "admin",
            NombreCompleto = "Administrador",
            IdTipoUsuario = 1
        });

        _factory.AuthServiceMock
            .Setup(s => s.LoginAsync(
                It.Is<LoginRequestDto>(r => r.Usuario == request.Usuario && r.Clave == request.Clave),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<LoginResponseDto>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos!.Token.Should().Be("jwt-token-test");
    }

    [Fact]
    public async Task Login_ConCredencialesInvalidas_RetornaBadRequest()
    {
        // Arrange
        _factory.AuthServiceMock.Reset();

        var request = new LoginRequestDto { Usuario = "admin", Clave = "wrong" };
        var expected = BaseResponseDto<LoginResponseDto>.Error("Usuario o clave incorrectos.");

        _factory.AuthServiceMock
            .Setup(s => s.LoginAsync(
                It.Is<LoginRequestDto>(r => r.Usuario == request.Usuario && r.Clave == request.Clave),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<LoginResponseDto>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeFalse();
        content.Mensaje.Should().Be("Usuario o clave incorrectos.");
    }

    [Fact]
    public async Task Login_CuandoServicioLanzaUnauthorizedException_RetornaUnauthorized()
    {
        // Arrange
        _factory.AuthServiceMock.Reset();

        var request = new LoginRequestDto { Usuario = "admin", Clave = "wrong" };

        _factory.AuthServiceMock
            .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new UnauthorizedException("Token inválido."));

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<object>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeFalse();
        content.Mensaje.Should().Be("Token inválido.");
    }

    [Fact]
    public async Task Login_CuandoServicioLanzaBusinessException_RetornaBadRequest()
    {
        // Arrange
        _factory.AuthServiceMock.Reset();

        var request = new LoginRequestDto { Usuario = "admin", Clave = "wrong" };

        _factory.AuthServiceMock
            .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new BusinessException("Error de negocio."));

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<object>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeFalse();
        content.Mensaje.Should().Be("Error de negocio.");
    }

    [Fact]
    public async Task Login_CuandoServicioLanzaExcepcionNoControlada_RetornaInternalServerError()
    {
        // Arrange
        _factory.AuthServiceMock.Reset();

        var request = new LoginRequestDto { Usuario = "admin", Clave = "wrong" };

        _factory.AuthServiceMock
            .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Boom"));

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<object>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeFalse();
        content.Mensaje.Should().Be("Ocurrió un error interno.");
    }
}
