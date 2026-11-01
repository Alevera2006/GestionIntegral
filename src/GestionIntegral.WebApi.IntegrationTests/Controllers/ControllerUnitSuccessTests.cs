using FluentAssertions;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Auth;
using GestionIntegral.Application.DTOs.Provincias;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GestionIntegral.WebApi.IntegrationTests.Controllers;

public class ControllerUnitSuccessTests
{
    [Fact]
    public async Task AuthController_Login_CuandoRespuestaExitoso_RetornaOk()
    {
        var authServiceMock = new Mock<IAuthService>();
        var request = new LoginRequestDto { Usuario = "admin", Clave = "admin" };
        var expected = BaseResponseDto<LoginResponseDto>.Ok(new LoginResponseDto
        {
            Token = "token",
            Usuario = "admin",
            NombreCompleto = "Administrador",
            IdTipoUsuario = 1
        });

        authServiceMock.Setup(s => s.LoginAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new AuthController(authServiceMock.Object);

        var result = await controller.Login(request, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var payload = ok.Value.Should().BeOfType<BaseResponseDto<LoginResponseDto>>().Subject;
        payload.Exitoso.Should().BeTrue();
        payload.Datos!.Token.Should().Be("token");
    }

    [Fact]
    public async Task ProvinciasController_Create_CuandoRespuestaExitoso_RetornaOk()
    {
        var provinciaServiceMock = new Mock<IProvinciaService>();
        var request = new CrearProvinciaRequestDto
        {
            Codigo = "SL",
            Descripcion = "San Luis",
            CodigoAFIP = "24"
        };
        var expected = BaseResponseDto<ProvinciaResponseDto>.Ok(new ProvinciaResponseDto
        {
            IdProvincia = 24,
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            CodigoAFIP = request.CodigoAFIP,
            Activo = true
        });

        provinciaServiceMock.Setup(s => s.CrearAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new ProvinciasController(provinciaServiceMock.Object);

        var result = await controller.Create(request, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var payload = ok.Value.Should().BeOfType<BaseResponseDto<ProvinciaResponseDto>>().Subject;
        payload.Exitoso.Should().BeTrue();
        payload.Datos!.IdProvincia.Should().Be(24);
    }
}
