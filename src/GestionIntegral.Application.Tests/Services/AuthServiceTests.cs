using FluentAssertions;
using GestionIntegral.Application.DTOs.Auth;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _service = new AuthService(_unitOfWorkMock.Object, _tokenServiceMock.Object, _passwordServiceMock.Object);
    }

    [Fact]
    public async Task LoginAsync_CredencialesValidas_RetornaToken()
    {
        var request = new LoginRequestDto { Usuario = "admin", Clave = "123" };
        var usuario = new Usuario
        {
            UsuarioNombre = "admin",
            ClaveHash = "HASH",
            Nombre = "Ada",
            Apellido = "Lovelace",
            IdTipoUsuario = 2,
            Activo = true,
            Bloqueado = false
        };

        _unitOfWorkMock.Setup(u => u.Usuarios.GetByUsuarioNombreAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(usuario);
        _passwordServiceMock.Setup(p => p.Verify("123", "HASH")).Returns(true);
        _tokenServiceMock.Setup(t => t.GenerarToken(usuario)).Returns("token-test");

        var result = await _service.LoginAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Token.Should().Be("token-test");
    }

    [Fact]
    public async Task LoginAsync_UsuarioNoExiste_RetornaError()
    {
        var request = new LoginRequestDto { Usuario = "admin", Clave = "123" };
        _unitOfWorkMock.Setup(u => u.Usuarios.GetByUsuarioNombreAsync(request.Usuario, It.IsAny<CancellationToken>())).ReturnsAsync((Usuario?)null);

        var result = await _service.LoginAsync(request);

        result.Exitoso.Should().BeFalse();
        result.Mensaje.Should().Contain("incorrectos");
    }

    [Fact]
    public async Task LoginAsync_UsuarioBloqueado_RetornaError()
    {
        var request = new LoginRequestDto { Usuario = "admin", Clave = "123" };
        var usuario = new Usuario { UsuarioNombre = "admin", ClaveHash = "HASH", Activo = true, Bloqueado = true };

        _unitOfWorkMock.Setup(u => u.Usuarios.GetByUsuarioNombreAsync(request.Usuario, It.IsAny<CancellationToken>())).ReturnsAsync(usuario);
        _passwordServiceMock.Setup(p => p.Verify(request.Clave, usuario.ClaveHash)).Returns(true);

        var result = await _service.LoginAsync(request);

        result.Exitoso.Should().BeFalse();
        result.Mensaje.Should().Contain("bloqueado");
    }

    [Fact]
    public async Task LoginAsync_UsuarioInactivo_RetornaError()
    {
        var request = new LoginRequestDto { Usuario = "admin", Clave = "123" };
        var usuario = new Usuario { UsuarioNombre = "admin", ClaveHash = "HASH", Activo = false, Bloqueado = false };

        _unitOfWorkMock.Setup(u => u.Usuarios.GetByUsuarioNombreAsync(request.Usuario, It.IsAny<CancellationToken>())).ReturnsAsync(usuario);
        _passwordServiceMock.Setup(p => p.Verify(request.Clave, usuario.ClaveHash)).Returns(true);

        var result = await _service.LoginAsync(request);

        result.Exitoso.Should().BeFalse();
        result.Mensaje.Should().Contain("inactivo");
    }
}
