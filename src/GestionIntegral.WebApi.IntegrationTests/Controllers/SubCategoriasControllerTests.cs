using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.SubCategorias;
using Moq;

namespace GestionIntegral.WebApi.IntegrationTests.Controllers;

public class SubCategoriasControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public SubCategoriasControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_RetornaOkConListado()
    {
        _factory.SubCategoriaServiceMock.Reset();

        var expectedItems = new List<SubCategoriaResponseDto>
        {
            new() { IdSubCategoria = 1, IdCategoria = 10, CategoriaDescripcion = "Bebidas", Codigo = "SUB1", Descripcion = "Gaseosas", Activo = true },
            new() { IdSubCategoria = 2, IdCategoria = 10, CategoriaDescripcion = "Bebidas", Codigo = "SUB2", Descripcion = "Jugos", Activo = true }
        };

        _factory.SubCategoriaServiceMock
            .Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<SubCategoriaResponseDto>>.Ok(expectedItems));

        var response = await _client.GetAsync("/api/SubCategorias");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<SubCategoriaResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos.Should().NotBeNull();
        content.Datos!.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByCategoria_RetornaOkConListadoFiltrado()
    {
        _factory.SubCategoriaServiceMock.Reset();

        const int idCategoria = 10;
        var expectedItems = new List<SubCategoriaResponseDto>
        {
            new() { IdSubCategoria = 1, IdCategoria = idCategoria, CategoriaDescripcion = "Bebidas", Codigo = "SUB1", Descripcion = "Gaseosas", Activo = true }
        };

        _factory.SubCategoriaServiceMock
            .Setup(s => s.ObtenerPorCategoriaAsync(idCategoria, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<SubCategoriaResponseDto>>.Ok(expectedItems));

        var response = await _client.GetAsync($"/api/SubCategorias/categoria/{idCategoria}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<SubCategoriaResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos.Should().NotBeNull();
        content.Datos!.Should().ContainSingle();
    }

    [Fact]
    public async Task GetById_RetornaOkConElemento()
    {
        _factory.SubCategoriaServiceMock.Reset();

        const int id = 1;
        var expected = new SubCategoriaResponseDto
        {
            IdSubCategoria = id,
            IdCategoria = 10,
            CategoriaDescripcion = "Bebidas",
            Codigo = "SUB1",
            Descripcion = "Gaseosas",
            Activo = true
        };

        _factory.SubCategoriaServiceMock
            .Setup(s => s.ObtenerPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<SubCategoriaResponseDto>.Ok(expected));

        var response = await _client.GetAsync($"/api/SubCategorias/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<SubCategoriaResponseDto>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos.Should().NotBeNull();
        content.Datos!.IdSubCategoria.Should().Be(id);
    }

    [Fact]
    public async Task Create_RetornaCreatedConElementoCreado()
    {
        _factory.SubCategoriaServiceMock.Reset();

        var request = new CrearSubCategoriaRequestDto
        {
            IdCategoria = 10,
            Codigo = "SUB3",
            Descripcion = "Aguas"
        };

        var created = new SubCategoriaResponseDto
        {
            IdSubCategoria = 3,
            IdCategoria = request.IdCategoria,
            CategoriaDescripcion = "Bebidas",
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Activo = true
        };

        _factory.SubCategoriaServiceMock
            .Setup(s => s.CrearAsync(
                It.Is<CrearSubCategoriaRequestDto>(r =>
                    r.IdCategoria == request.IdCategoria &&
                    r.Codigo == request.Codigo &&
                    r.Descripcion == request.Descripcion),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<SubCategoriaResponseDto>.Ok(created));

        var response = await _client.PostAsJsonAsync("/api/SubCategorias", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<SubCategoriaResponseDto>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos.Should().NotBeNull();
        content.Datos!.IdSubCategoria.Should().Be(3);
    }

    [Fact]
    public async Task Update_RetornaOkConElementoActualizado()
    {
        _factory.SubCategoriaServiceMock.Reset();

        var request = new ActualizarSubCategoriaRequestDto
        {
            IdSubCategoria = 1,
            IdCategoria = 10,
            Codigo = "SUB1",
            Descripcion = "Gaseosas (actualizado)",
            Activo = true
        };

        var updated = new SubCategoriaResponseDto
        {
            IdSubCategoria = request.IdSubCategoria,
            IdCategoria = request.IdCategoria,
            CategoriaDescripcion = "Bebidas",
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Activo = request.Activo
        };

        _factory.SubCategoriaServiceMock
            .Setup(s => s.ActualizarAsync(
                It.Is<ActualizarSubCategoriaRequestDto>(r =>
                    r.IdSubCategoria == request.IdSubCategoria &&
                    r.IdCategoria == request.IdCategoria &&
                    r.Codigo == request.Codigo &&
                    r.Descripcion == request.Descripcion &&
                    r.Activo == request.Activo),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<SubCategoriaResponseDto>.Ok(updated));

        var response = await _client.PutAsJsonAsync("/api/SubCategorias", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<SubCategoriaResponseDto>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos.Should().NotBeNull();
        content.Datos!.Descripcion.Should().Be("Gaseosas (actualizado)");
    }

    [Fact]
    public async Task Delete_RetornaOkConTrue()
    {
        _factory.SubCategoriaServiceMock.Reset();

        const int id = 1;

        _factory.SubCategoriaServiceMock
            .Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/SubCategorias/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<bool>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos.Should().BeTrue();
    }
}
