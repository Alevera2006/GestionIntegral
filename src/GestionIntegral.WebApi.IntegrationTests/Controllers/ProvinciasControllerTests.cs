using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Provincias;
using Moq;

namespace GestionIntegral.WebApi.IntegrationTests.Controllers;

public class ProvinciasControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ProvinciasControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_RetornaOkConListado()
    {
        // Arrange
        _factory.ProvinciaServiceMock.Reset();

        var expectedItems = new List<ProvinciaResponseDto>
        {
            new() { IdProvincia = 1, Codigo = "BA", Descripcion = "Buenos Aires", CodigoAFIP = "01", Activo = true },
            new() { IdProvincia = 2, Codigo = "CBA", Descripcion = "Córdoba", CodigoAFIP = "02", Activo = true }
        };

        _factory.ProvinciaServiceMock
            .Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<ProvinciaResponseDto>>.Ok(expectedItems));

        // Act
        var response = await _client.GetAsync("/api/Provincias");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<ProvinciaResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos.Should().NotBeNull();
        content.Datos!.Should().HaveCount(2);
    }

    [Fact]
    public async Task Create_RetornaOkConElementoCreado()
    {
        // Arrange
        _factory.ProvinciaServiceMock.Reset();

        var request = new CrearProvinciaRequestDto
        {
            Codigo = "SL",
            Descripcion = "San Luis",
            CodigoAFIP = "24"
        };

        var created = new ProvinciaResponseDto
        {
            IdProvincia = 24,
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            CodigoAFIP = request.CodigoAFIP,
            Activo = true
        };

        _factory.ProvinciaServiceMock
            .Setup(s => s.CrearAsync(
                It.Is<CrearProvinciaRequestDto>(r =>
                    r.Codigo == request.Codigo &&
                    r.Descripcion == request.Descripcion &&
                    r.CodigoAFIP == request.CodigoAFIP),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<ProvinciaResponseDto>.Ok(created));

        // Act
        var response = await _client.PostAsJsonAsync("/api/Provincias", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<ProvinciaResponseDto>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos.Should().NotBeNull();
        content.Datos!.IdProvincia.Should().Be(24);
        content.Datos.Codigo.Should().Be("SL");
    }

    [Fact]
    public async Task Update_RetornaOkConElementoActualizado()
    {
        // Arrange
        _factory.ProvinciaServiceMock.Reset();

        var request = new ActualizarProvinciaRequestDto
        {
            IdProvincia = 1,
            Codigo = "BA",
            Descripcion = "Buenos Aires (actualizado)",
            CodigoAFIP = "01",
            Activo = true
        };

        var updated = new ProvinciaResponseDto
        {
            IdProvincia = request.IdProvincia,
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            CodigoAFIP = request.CodigoAFIP,
            Activo = request.Activo
        };

        _factory.ProvinciaServiceMock
            .Setup(s => s.ActualizarAsync(
                It.Is<ActualizarProvinciaRequestDto>(r =>
                    r.IdProvincia == request.IdProvincia &&
                    r.Codigo == request.Codigo &&
                    r.Descripcion == request.Descripcion &&
                    r.CodigoAFIP == request.CodigoAFIP &&
                    r.Activo == request.Activo),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<ProvinciaResponseDto>.Ok(updated));

        // Act
        var response = await _client.PutAsJsonAsync("/api/Provincias", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<ProvinciaResponseDto>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos.Should().NotBeNull();
        content.Datos!.Descripcion.Should().Be("Buenos Aires (actualizado)");
    }

    [Fact]
    public async Task Delete_RetornaOkConTrue()
    {
        // Arrange
        _factory.ProvinciaServiceMock.Reset();

        const int id = 1;

        _factory.ProvinciaServiceMock
            .Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        // Act
        var response = await _client.DeleteAsync($"/api/Provincias/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<bool>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos.Should().BeTrue();
    }
}
