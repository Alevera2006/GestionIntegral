using FluentAssertions;
using GestionIntegral.WebClient.Controllers;
using GestionIntegral.WebClient.Models.Stock;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GestionIntegral.WebClient.Tests.Controllers;

public class ReportesControllerTests
{
    private readonly Mock<IApiClient> _apiClientMock = new();

    private ReportesController CreateController()
    {
        return new ReportesController(_apiClientMock.Object);
    }

    [Fact]
    public void Index_DebeRetornarCuatroCategorias()
    {
        var controller = CreateController();

        var result = controller.Index();

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<List<OpcionStockViewModel>>().Subject;
        model.Should().HaveCount(4);
        model.Select(m => m.Titulo).Should().ContainInOrder("Ventas", "Compras", "Stock", "Finanzas");
    }

    [Fact]
    public void Ventas_DebeRetornarReportesDeVentas()
    {
        var controller = CreateController();

        var result = controller.Ventas();

        var model = (result as ViewResult)!.Model as List<OpcionStockViewModel>;
        model.Should().HaveCount(4);
        model!.Select(m => m.Titulo).Should().Contain("Ventas por Período", "Ventas por Cliente", "Ventas por Producto", "Ranking de Productos");
    }

    [Fact]
    public void Compras_DebeRetornarReportesDeCompras()
    {
        var controller = CreateController();

        var result = controller.Compras();

        var model = (result as ViewResult)!.Model as List<OpcionStockViewModel>;
        model.Should().HaveCount(2);
        model!.Select(m => m.Titulo).Should().Contain("Compras por Período", "Compras por Proveedor");
    }

    [Fact]
    public void Stock_DebeRetornarReportesDeStock()
    {
        var controller = CreateController();

        var result = controller.Stock();

        var model = (result as ViewResult)!.Model as List<OpcionStockViewModel>;
        model.Should().HaveCount(3);
        model!.Select(m => m.Titulo).Should().Contain("Stock por Depósito", "Movimientos de Stock", "Alertas de Stock");
    }

    [Fact]
    public void Finanzas_DebeRetornarReportesDeFinanzas()
    {
        var controller = CreateController();

        var result = controller.Finanzas();

        var model = (result as ViewResult)!.Model as List<OpcionStockViewModel>;
        model.Should().HaveCount(4);
        model!.Select(m => m.Titulo).Should().Contain("Cuentas Corrientes", "Cobranzas", "Valores en Cartera", "Deuda por Cliente");
    }

    [Theory]
    [InlineData("VentasPorPeriodo", "Ventas por Período")]
    [InlineData("VentasPorCliente", "Ventas por Cliente")]
    [InlineData("VentasPorProducto", "Ventas por Producto")]
    [InlineData("RankingProductos", "Ranking de Productos")]
    [InlineData("ComprasPorPeriodo", "Compras por Período")]
    [InlineData("ComprasPorProveedor", "Compras por Proveedor")]
    [InlineData("StockPorDeposito", "Stock por Depósito")]
    [InlineData("MovimientosStock", "Movimientos de Stock")]
    [InlineData("AlertasStock", "Alertas de Stock")]
    [InlineData("CuentasCorrientes", "Cuentas Corrientes")]
    [InlineData("Cobranzas", "Cobranzas")]
    [InlineData("ValoresCartera", "Valores en Cartera")]
    [InlineData("DeudaPorCliente", "Deuda por Cliente")]
    public void ReportesPlaceholder_DebenRetornarVistaPlaceholderConTituloCorrecto(string actionName, string tituloEsperado)
    {
        var controller = CreateController();

        var result = controller.GetType().GetMethod(actionName)!.Invoke(controller, null) as ViewResult;

        result.Should().NotBeNull();
        result!.ViewName.Should().Be("ReportePlaceholder");
        var model = result.Model as (string Titulo, string Descripcion)?;
        model.Should().NotBeNull();
        model!.Value.Titulo.Should().Be(tituloEsperado);
    }
}
