using System.Net;
using System.Text.Json;
using FluentAssertions;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.WebApi.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace GestionIntegral.WebApi.IntegrationTests.Middleware;

public class ErrorHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock = new();

    [Fact]
    public async Task InvokeAsync_CuandoNoHayExcepcion_EjecutaSiguienteMiddleware()
    {
        var fueEjecutado = false;
        RequestDelegate next = _ =>
        {
            fueEjecutado = true;
            return Task.CompletedTask;
        };

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object);
        var context = CrearHttpContext();

        await middleware.InvokeAsync(context);

        fueEjecutado.Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_CuandoLanzaNotFoundException_Retorna404YMensaje()
    {
        var response = await EjecutarConExcepcion(new NotFoundException("Categoria", 99));

        response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        response.ContentType.Should().Be("application/json");

        var body = await LeerBody(response.HttpContext);
        body.Exitoso.Should().BeFalse();
        body.Mensaje.Should().Be("Entidad 'Categoria' con clave '99' no encontrada.");
    }

    [Fact]
    public async Task InvokeAsync_CuandoLanzaBusinessException_Retorna400YMensaje()
    {
        var response = await EjecutarConExcepcion(new BusinessException("Error de negocio"));

        response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        response.ContentType.Should().Be("application/json");

        var body = await LeerBody(response.HttpContext);
        body.Exitoso.Should().BeFalse();
        body.Mensaje.Should().Be("Error de negocio");
    }

    [Fact]
    public async Task InvokeAsync_CuandoLanzaUnauthorizedException_Retorna401YMensaje()
    {
        var response = await EjecutarConExcepcion(new UnauthorizedException("No autorizado"));

        response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        response.ContentType.Should().Be("application/json");

        var body = await LeerBody(response.HttpContext);
        body.Exitoso.Should().BeFalse();
        body.Mensaje.Should().Be("No autorizado");
    }

    [Fact]
    public async Task InvokeAsync_CuandoLanzaExcepcionNoControlada_Retorna500YMensajeGenerico()
    {
        var response = await EjecutarConExcepcion(new Exception("boom"));

        response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        response.ContentType.Should().Be("application/json");

        var body = await LeerBody(response.HttpContext);
        body.Exitoso.Should().BeFalse();
        body.Mensaje.Should().Be("Ocurrió un error interno.");
    }

    private async Task<HttpResponse> EjecutarConExcepcion(Exception exception)
    {
        RequestDelegate next = _ => throw exception;

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object);
        var context = CrearHttpContext();

        await middleware.InvokeAsync(context);
        return context.Response;
    }

    private static DefaultHttpContext CrearHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<ErrorResponse> LeerBody(HttpContext context)
    {
        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body);
        var json = await reader.ReadToEndAsync();

        var body = JsonSerializer.Deserialize<ErrorResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        body.Should().NotBeNull();
        return body!;
    }

    private sealed class ErrorResponse
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}