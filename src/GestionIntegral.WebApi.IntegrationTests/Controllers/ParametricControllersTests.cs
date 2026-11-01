using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.AlicuotasImpuesto;
using GestionIntegral.Application.DTOs.Categorias;
using GestionIntegral.Application.DTOs.CondicionesIVA;
using GestionIntegral.Application.DTOs.CondicionesPago;
using GestionIntegral.Application.DTOs.Depositos;
using GestionIntegral.Application.DTOs.EstadosCaja;
using GestionIntegral.Application.DTOs.EstadosCompra;
using GestionIntegral.Application.DTOs.EstadosFactura;
using GestionIntegral.Application.DTOs.EstadosPedido;
using GestionIntegral.Application.DTOs.EstadosProceso;
using GestionIntegral.Application.DTOs.MediosPago;
using GestionIntegral.Application.DTOs.Monedas;
using GestionIntegral.Application.DTOs.MotivosAnulacion;
using GestionIntegral.Application.DTOs.TiposComprobante;
using GestionIntegral.Application.DTOs.TiposImpuesto;
using GestionIntegral.Application.DTOs.TiposMovimientoCaja;
using GestionIntegral.Application.DTOs.TiposMovimientoStock;
using GestionIntegral.Application.DTOs.TiposUsuario;
using Moq;

namespace GestionIntegral.WebApi.IntegrationTests.Controllers;

public class ParametricControllersTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ParametricControllersTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AlicuotasImpuesto_GetAll_RetornaOk()
    {
        _factory.AlicuotaImpuestoServiceMock.Reset();
        _factory.AlicuotaImpuestoServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<AlicuotaImpuestoResponseDto>>.Ok(Array.Empty<AlicuotaImpuestoResponseDto>()));

        var response = await _client.GetAsync("/api/AlicuotasImpuesto");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<AlicuotaImpuestoResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task Categorias_GetAll_RetornaOk()
    {
        _factory.CategoriaServiceMock.Reset();
        _factory.CategoriaServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<CategoriaResponseDto>>.Ok(Array.Empty<CategoriaResponseDto>()));

        var response = await _client.GetAsync("/api/Categorias");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<CategoriaResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task CondicionesIVA_GetAll_RetornaOk()
    {
        _factory.CondicionIvaServiceMock.Reset();
        _factory.CondicionIvaServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<CondicionIVAResponseDto>>.Ok(Array.Empty<CondicionIVAResponseDto>()));

        var response = await _client.GetAsync("/api/CondicionesIVA");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<CondicionIVAResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task CondicionesPago_GetAll_RetornaOk()
    {
        _factory.CondicionPagoServiceMock.Reset();
        _factory.CondicionPagoServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<CondicionPagoResponseDto>>.Ok(Array.Empty<CondicionPagoResponseDto>()));

        var response = await _client.GetAsync("/api/CondicionesPago");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<CondicionPagoResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task Depositos_GetAll_RetornaOk()
    {
        _factory.DepositoServiceMock.Reset();
        _factory.DepositoServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<DepositoResponseDto>>.Ok(Array.Empty<DepositoResponseDto>()));

        var response = await _client.GetAsync("/api/Depositos");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<DepositoResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task EstadosCaja_GetAll_RetornaOk()
    {
        _factory.EstadoCajaServiceMock.Reset();
        _factory.EstadoCajaServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<EstadoCajaResponseDto>>.Ok(Array.Empty<EstadoCajaResponseDto>()));

        var response = await _client.GetAsync("/api/EstadosCaja");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<EstadoCajaResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task EstadosCompra_GetAll_RetornaOk()
    {
        _factory.EstadoCompraServiceMock.Reset();
        _factory.EstadoCompraServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<EstadoCompraResponseDto>>.Ok(Array.Empty<EstadoCompraResponseDto>()));

        var response = await _client.GetAsync("/api/EstadosCompra");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<EstadoCompraResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task EstadosFactura_GetAll_RetornaOk()
    {
        _factory.EstadoFacturaServiceMock.Reset();
        _factory.EstadoFacturaServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<EstadoFacturaResponseDto>>.Ok(Array.Empty<EstadoFacturaResponseDto>()));

        var response = await _client.GetAsync("/api/EstadosFactura");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<EstadoFacturaResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task EstadosPedido_GetAll_RetornaOk()
    {
        _factory.EstadoPedidoServiceMock.Reset();
        _factory.EstadoPedidoServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<EstadoPedidoResponseDto>>.Ok(Array.Empty<EstadoPedidoResponseDto>()));

        var response = await _client.GetAsync("/api/EstadosPedido");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<EstadoPedidoResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task EstadosProceso_GetAll_RetornaOk()
    {
        _factory.EstadoProcesoServiceMock.Reset();
        _factory.EstadoProcesoServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<EstadoProcesoResponseDto>>.Ok(Array.Empty<EstadoProcesoResponseDto>()));

        var response = await _client.GetAsync("/api/EstadosProceso");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<EstadoProcesoResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task MediosPago_GetAll_RetornaOk()
    {
        _factory.MedioPagoServiceMock.Reset();
        _factory.MedioPagoServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<MedioPagoResponseDto>>.Ok(Array.Empty<MedioPagoResponseDto>()));

        var response = await _client.GetAsync("/api/MediosPago");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<MedioPagoResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task Monedas_GetAll_RetornaOk()
    {
        _factory.MonedaServiceMock.Reset();
        _factory.MonedaServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<MonedaResponseDto>>.Ok(Array.Empty<MonedaResponseDto>()));

        var response = await _client.GetAsync("/api/Monedas");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<MonedaResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task MotivosAnulacion_GetAll_RetornaOk()
    {
        _factory.MotivoAnulacionServiceMock.Reset();
        _factory.MotivoAnulacionServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<MotivoAnulacionResponseDto>>.Ok(Array.Empty<MotivoAnulacionResponseDto>()));

        var response = await _client.GetAsync("/api/MotivosAnulacion");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<MotivoAnulacionResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task TiposComprobante_GetAll_RetornaOk()
    {
        _factory.TipoComprobanteServiceMock.Reset();
        _factory.TipoComprobanteServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<TipoComprobanteResponseDto>>.Ok(Array.Empty<TipoComprobanteResponseDto>()));

        var response = await _client.GetAsync("/api/TiposComprobante");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<TipoComprobanteResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task TiposImpuesto_GetAll_RetornaOk()
    {
        _factory.TipoImpuestoServiceMock.Reset();
        _factory.TipoImpuestoServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<TipoImpuestoResponseDto>>.Ok(Array.Empty<TipoImpuestoResponseDto>()));

        var response = await _client.GetAsync("/api/TiposImpuesto");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<TipoImpuestoResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task TiposMovimientoCaja_GetAll_RetornaOk()
    {
        _factory.TipoMovimientoCajaServiceMock.Reset();
        _factory.TipoMovimientoCajaServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<TipoMovimientoCajaResponseDto>>.Ok(Array.Empty<TipoMovimientoCajaResponseDto>()));

        var response = await _client.GetAsync("/api/TiposMovimientoCaja");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<TipoMovimientoCajaResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task TiposMovimientoStock_GetAll_RetornaOk()
    {
        _factory.TipoMovimientoStockServiceMock.Reset();
        _factory.TipoMovimientoStockServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<TipoMovimientoStockResponseDto>>.Ok(Array.Empty<TipoMovimientoStockResponseDto>()));

        var response = await _client.GetAsync("/api/TiposMovimientoStock");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<TipoMovimientoStockResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task TiposUsuario_GetAll_RetornaOk()
    {
        _factory.TipoUsuarioServiceMock.Reset();
        _factory.TipoUsuarioServiceMock.Setup(s => s.ObtenerTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<IEnumerable<TipoUsuarioResponseDto>>.Ok(Array.Empty<TipoUsuarioResponseDto>()));

        var response = await _client.GetAsync("/api/TiposUsuario");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<BaseResponseDto<IEnumerable<TipoUsuarioResponseDto>>>(_jsonOptions);
        content.Should().NotBeNull();
        content!.Exitoso.Should().BeTrue();
    }

    [Fact]
    public async Task AlicuotasImpuesto_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.AlicuotaImpuestoServiceMock.Reset();
        _factory.AlicuotaImpuestoServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/AlicuotasImpuesto/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Categorias_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.CategoriaServiceMock.Reset();
        _factory.CategoriaServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/Categorias/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CondicionesIVA_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.CondicionIvaServiceMock.Reset();
        _factory.CondicionIvaServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/CondicionesIVA/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CondicionesPago_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.CondicionPagoServiceMock.Reset();
        _factory.CondicionPagoServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/CondicionesPago/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Depositos_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.DepositoServiceMock.Reset();
        _factory.DepositoServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/Depositos/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task EstadosCaja_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.EstadoCajaServiceMock.Reset();
        _factory.EstadoCajaServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/EstadosCaja/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task EstadosCompra_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.EstadoCompraServiceMock.Reset();
        _factory.EstadoCompraServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/EstadosCompra/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task EstadosFactura_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.EstadoFacturaServiceMock.Reset();
        _factory.EstadoFacturaServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/EstadosFactura/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task EstadosPedido_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.EstadoPedidoServiceMock.Reset();
        _factory.EstadoPedidoServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/EstadosPedido/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task EstadosProceso_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.EstadoProcesoServiceMock.Reset();
        _factory.EstadoProcesoServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/EstadosProceso/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task MediosPago_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.MedioPagoServiceMock.Reset();
        _factory.MedioPagoServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/MediosPago/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Monedas_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.MonedaServiceMock.Reset();
        _factory.MonedaServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/Monedas/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task MotivosAnulacion_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.MotivoAnulacionServiceMock.Reset();
        _factory.MotivoAnulacionServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/MotivosAnulacion/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TiposComprobante_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.TipoComprobanteServiceMock.Reset();
        _factory.TipoComprobanteServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/TiposComprobante/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TiposImpuesto_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.TipoImpuestoServiceMock.Reset();
        _factory.TipoImpuestoServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/TiposImpuesto/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TiposMovimientoCaja_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.TipoMovimientoCajaServiceMock.Reset();
        _factory.TipoMovimientoCajaServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/TiposMovimientoCaja/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TiposMovimientoStock_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.TipoMovimientoStockServiceMock.Reset();
        _factory.TipoMovimientoStockServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/TiposMovimientoStock/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TiposUsuario_Delete_RetornaOk()
    {
        const int id = 1;
        _factory.TipoUsuarioServiceMock.Reset();
        _factory.TipoUsuarioServiceMock.Setup(s => s.EliminarAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BaseResponseDto<bool>.Ok(true));

        var response = await _client.DeleteAsync($"/api/TiposUsuario/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
