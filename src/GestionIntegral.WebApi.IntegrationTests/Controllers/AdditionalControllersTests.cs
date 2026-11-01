using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Cajas;
using GestionIntegral.Application.DTOs.ClienteContactos;
using GestionIntegral.Application.DTOs.Clientes;
using GestionIntegral.Application.DTOs.Contactos;
using GestionIntegral.Application.DTOs.Domicilios;
using GestionIntegral.Application.DTOs.Emails;
using GestionIntegral.Application.DTOs.ListaPrecioDetalles;
using GestionIntegral.Application.DTOs.ListasPrecios;
using GestionIntegral.Application.DTOs.MovimientosStock;
using GestionIntegral.Application.DTOs.Productos;
using GestionIntegral.Application.DTOs.Proveedores;
using GestionIntegral.Application.DTOs.Stock;
using GestionIntegral.Application.DTOs.Telefonos;
using GestionIntegral.Application.DTOs.Usuarios;
using Moq;

namespace GestionIntegral.WebApi.IntegrationTests.Controllers;

public class AdditionalControllersTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public AdditionalControllersTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Cajas_GetAll_RetornaOk()
    {
        _factory.CajaCabeceraServiceMock.Reset();
        _factory.CajaCabeceraServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<CajaCabeceraResponseDto>>.Ok(Array.Empty<CajaCabeceraResponseDto>()));

        var response = await _client.GetAsync("/api/Cajas");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Cajas_GetDetalles_RetornaOk()
    {
        _factory.CajaDetalleServiceMock.Reset();
        _factory.CajaDetalleServiceMock.Setup(s => s.ObtenerPorCajaAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<CajaDetalleResponseDto>>.Ok(Array.Empty<CajaDetalleResponseDto>()));

        var response = await _client.GetAsync("/api/Cajas/1/detalles");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Clientes_GetAll_RetornaOk()
    {
        _factory.ClienteServiceMock.Reset();
        _factory.ClienteServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<ClienteResponseDto>>.Ok(Array.Empty<ClienteResponseDto>()));

        var response = await _client.GetAsync("/api/Clientes");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Proveedores_GetAll_RetornaOk()
    {
        _factory.ProveedorServiceMock.Reset();
        _factory.ProveedorServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<ProveedorResponseDto>>.Ok(Array.Empty<ProveedorResponseDto>()));

        var response = await _client.GetAsync("/api/Proveedores");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Proveedores_GetById_RetornaOkConElemento()
    {
        const int id = 7;
        var expected = new ProveedorResponseDto { IdProveedor = id, Codigo = "PRV-007", RazonSocial = "Proveedor Test", Activo = true };

        _factory.ProveedorServiceMock.Reset();
        _factory.ProveedorServiceMock.Setup(s => s.ObtenerPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<ProveedorResponseDto>.Ok(expected));

        var response = await _client.GetAsync($"/api/Proveedores/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<ProveedorResponseDto>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos!.IdProveedor.Should().Be(id);
    }

    [Fact]
    public async Task Productos_GetAll_RetornaOk()
    {
        _factory.ProductoServiceMock.Reset();
        _factory.ProductoServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<ProductoResponseDto>>.Ok(Array.Empty<ProductoResponseDto>()));

        var response = await _client.GetAsync("/api/Productos");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Usuarios_GetAll_RetornaOk()
    {
        _factory.UsuarioServiceMock.Reset();
        _factory.UsuarioServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<UsuarioResponseDto>>.Ok(Array.Empty<UsuarioResponseDto>()));

        var response = await _client.GetAsync("/api/Usuarios");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Usuarios_GetById_RetornaOkConElemento()
    {
        const int id = 10;
        var expected = new UsuarioResponseDto
        {
            IdUsuario = id,
            Nombre = "Admin",
            Apellido = "Sistema",
            UsuarioNombre = "admin",
            IdTipoUsuario = 1,
            TipoUsuarioDescripcion = "Administrador",
            Activo = true
        };

        _factory.UsuarioServiceMock.Reset();
        _factory.UsuarioServiceMock.Setup(s => s.ObtenerPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<UsuarioResponseDto>.Ok(expected));

        var response = await _client.GetAsync($"/api/Usuarios/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<UsuarioResponseDto>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos!.IdUsuario.Should().Be(id);
    }

    [Fact]
    public async Task ClienteContactos_GetByCliente_RetornaOk()
    {
        _factory.ClienteContactoServiceMock.Reset();
        _factory.ClienteContactoServiceMock.Setup(s => s.ObtenerPorClienteAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<ClienteContactoResponseDto>>.Ok(Array.Empty<ClienteContactoResponseDto>()));

        var response = await _client.GetAsync("/api/ClienteContactos/cliente/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Contactos_GetAll_RetornaOk()
    {
        _factory.ContactoServiceMock.Reset();
        _factory.ContactoServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<ContactoResponseDto>>.Ok(Array.Empty<ContactoResponseDto>()));

        var response = await _client.GetAsync("/api/Contactos");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Domicilios_GetAll_RetornaOk()
    {
        _factory.DomicilioServiceMock.Reset();
        _factory.DomicilioServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<DomicilioResponseDto>>.Ok(Array.Empty<DomicilioResponseDto>()));

        var response = await _client.GetAsync("/api/Domicilios");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Emails_GetAll_RetornaOk()
    {
        _factory.EmailServiceMock.Reset();
        _factory.EmailServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<EmailResponseDto>>.Ok(Array.Empty<EmailResponseDto>()));

        var response = await _client.GetAsync("/api/Emails");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Telefonos_GetAll_RetornaOk()
    {
        _factory.TelefonoServiceMock.Reset();
        _factory.TelefonoServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<TelefonoResponseDto>>.Ok(Array.Empty<TelefonoResponseDto>()));

        var response = await _client.GetAsync("/api/Telefonos");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ListasPrecios_GetAll_RetornaOk()
    {
        _factory.ListaPrecioServiceMock.Reset();
        _factory.ListaPrecioServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<ListaPrecioResponseDto>>.Ok(Array.Empty<ListaPrecioResponseDto>()));

        var response = await _client.GetAsync("/api/ListasPrecios");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ListasPrecios_GetDetalles_RetornaOk()
    {
        _factory.ListaPrecioServiceMock.Reset();
        _factory.ListaPrecioServiceMock.Setup(s => s.ObtenerDetallesAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<ListaPrecioDetalleResponseDto>>.Ok(Array.Empty<ListaPrecioDetalleResponseDto>()));

        var response = await _client.GetAsync("/api/ListasPrecios/1/detalles");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task MovimientosStock_GetAll_RetornaOk()
    {
        _factory.MovimientoStockServiceMock.Reset();
        _factory.MovimientoStockServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<MovimientoStockResponseDto>>.Ok(Array.Empty<MovimientoStockResponseDto>()));

        var response = await _client.GetAsync("/api/MovimientosStock");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task MovimientosStock_GetById_RetornaOkConElemento()
    {
        const int id = 3;
        var expected = new MovimientoStockResponseDto
        {
            IdMovimientoStock = id,
            IdDeposito = 1,
            IdTipoMovimientoStock = 2,
            IdUsuarioResponsable = 3,
            Referencia = "MOV-003",
            Observaciones = "Ingreso inicial"
        };

        _factory.MovimientoStockServiceMock.Reset();
        _factory.MovimientoStockServiceMock.Setup(s => s.ObtenerPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<MovimientoStockResponseDto>.Ok(expected));

        var response = await _client.GetAsync($"/api/MovimientosStock/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<MovimientoStockResponseDto>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
        content.Datos!.IdMovimientoStock.Should().Be(id);
    }

    [Fact]
    public async Task MovimientosStock_GetDetalles_RetornaOk()
    {
        _factory.MovimientoStockDetalleServiceMock.Reset();
        _factory.MovimientoStockDetalleServiceMock.Setup(s => s.ObtenerPorMovimientoAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<MovimientoStockDetalleResponseDto>>.Ok(Array.Empty<MovimientoStockDetalleResponseDto>()));

        var response = await _client.GetAsync("/api/MovimientosStock/1/detalles");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Stock_GetAll_RetornaOk()
    {
        _factory.StockServiceMock.Reset();
        _factory.StockServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<StockResponseDto>>.Ok(Array.Empty<StockResponseDto>()));

        var response = await _client.GetAsync("/api/Stock");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}