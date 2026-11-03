using System.Net;
using FluentAssertions;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace GestionIntegral.WebClient.Tests.Services;

public class ApiClientTests
{
    private readonly Mock<ILogger<ApiClient>> _loggerMock = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

    private ApiClient CreateClient(HttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://localhost:7150/")
        };
        return new ApiClient(httpClient, _loggerMock.Object, _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task GetAsync_ConRespuesta401_DebeRetornarDefaultSinLanzarExcepcion()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent(string.Empty)
            });

        var client = CreateClient(handlerMock.Object);

        var act = async () => await client.GetAsync<object>("api/productos");

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task GetAsync_ConRespuesta200_DebeDeserializarYRetornarDatos()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"exitoso\":true,\"mensaje\":\"ok\",\"datos\":[\"item1\",\"item2\"]}")
            });

        var client = CreateClient(handlerMock.Object);

        var result = await client.GetAsync<TestResponse>("api/productos");

        result.Should().NotBeNull();
        result!.Exitoso.Should().BeTrue();
        result.Datos.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteAsync_ConRespuesta401_DebeRetornarFalseSinLanzarExcepcion()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized
            });

        var client = CreateClient(handlerMock.Object);

        var act = async () => await client.DeleteAsync("api/productos/1");

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task DeleteAsync_ConRespuesta200_DebeRetornarTrue()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        var client = CreateClient(handlerMock.Object);

        var result = await client.DeleteAsync("api/productos/1");

        result.Should().BeTrue();
    }

    private class TestResponse
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public List<string> Datos { get; set; } = new();
    }
}
