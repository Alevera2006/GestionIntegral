using FluentAssertions;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Domain.Interfaces.Services;
using GestionIntegral.WebApi.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GestionIntegral.WebApi.IntegrationTests.Controllers;

public class UtilidadesControllerTests
{
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<IWebHostEnvironment> _environmentMock = new();

    [Fact]
    public void GenerarHash_CuandoNoEsDevelopment_RetornaNotFound()
    {
        _environmentMock.SetupGet(x => x.EnvironmentName).Returns("Production");
        var controller = new UtilidadesController(_passwordServiceMock.Object, _environmentMock.Object);

        var result = controller.GenerarHash(new GenerarHashRequestDto { Clave = "abc123" });

        result.Should().BeOfType<NotFoundResult>();
        _passwordServiceMock.Verify(x => x.Hash(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void GenerarHash_CuandoClaveVacia_RetornaBadRequest()
    {
        _environmentMock.SetupGet(x => x.EnvironmentName).Returns("Development");
        var controller = new UtilidadesController(_passwordServiceMock.Object, _environmentMock.Object);

        var result = controller.GenerarHash(new GenerarHashRequestDto { Clave = "" });

        var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var payload = badRequest.Value.Should().BeOfType<BaseResponseDto<string>>().Subject;
        payload.Exitoso.Should().BeFalse();
        payload.Mensaje.Should().Be("La clave es requerida.");
        _passwordServiceMock.Verify(x => x.Hash(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void GenerarHash_CuandoRequestEsValida_RetornaOkConHash()
    {
        const string clave = "admin123";
        const string hash = "$2a$12$abcdefghijklmnopqrstuv";

        _environmentMock.SetupGet(x => x.EnvironmentName).Returns("Development");
        _passwordServiceMock.Setup(x => x.Hash(clave)).Returns(hash);

        var controller = new UtilidadesController(_passwordServiceMock.Object, _environmentMock.Object);

        var result = controller.GenerarHash(new GenerarHashRequestDto { Clave = clave });

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var payload = ok.Value.Should().BeOfType<BaseResponseDto<string>>().Subject;
        payload.Exitoso.Should().BeTrue();
        payload.Mensaje.Should().Be("Hash generado correctamente.");
        payload.Datos.Should().Be(hash);
        _passwordServiceMock.Verify(x => x.Hash(clave), Times.Once);
    }
}