using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GestionIntegral.WebApi.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IAuthService> AuthServiceMock { get; } = new();
    public Mock<IProvinciaService> ProvinciaServiceMock { get; } = new();
    public Mock<ISubCategoriaService> SubCategoriaServiceMock { get; } = new();
    public Mock<IAlicuotaImpuestoService> AlicuotaImpuestoServiceMock { get; } = new();
    public Mock<ICategoriaService> CategoriaServiceMock { get; } = new();
    public Mock<ICondicionIVAService> CondicionIvaServiceMock { get; } = new();
    public Mock<ICondicionPagoService> CondicionPagoServiceMock { get; } = new();
    public Mock<IDepositoService> DepositoServiceMock { get; } = new();
    public Mock<IEstadoCajaService> EstadoCajaServiceMock { get; } = new();
    public Mock<IEstadoCompraService> EstadoCompraServiceMock { get; } = new();
    public Mock<IEstadoFacturaService> EstadoFacturaServiceMock { get; } = new();
    public Mock<IEstadoPedidoService> EstadoPedidoServiceMock { get; } = new();
    public Mock<IEstadoProcesoService> EstadoProcesoServiceMock { get; } = new();
    public Mock<IMedioPagoService> MedioPagoServiceMock { get; } = new();
    public Mock<IMonedaService> MonedaServiceMock { get; } = new();
    public Mock<IMotivoAnulacionService> MotivoAnulacionServiceMock { get; } = new();
    public Mock<ITipoComprobanteService> TipoComprobanteServiceMock { get; } = new();
    public Mock<ITipoImpuestoService> TipoImpuestoServiceMock { get; } = new();
    public Mock<ITipoMovimientoCajaService> TipoMovimientoCajaServiceMock { get; } = new();
    public Mock<ITipoMovimientoStockService> TipoMovimientoStockServiceMock { get; } = new();
    public Mock<ITipoUsuarioService> TipoUsuarioServiceMock { get; } = new();
    public Mock<ICajaCabeceraService> CajaCabeceraServiceMock { get; } = new();
    public Mock<ICajaDetalleService> CajaDetalleServiceMock { get; } = new();
    public Mock<IClienteService> ClienteServiceMock { get; } = new();
    public Mock<IProveedorService> ProveedorServiceMock { get; } = new();
    public Mock<IProductoService> ProductoServiceMock { get; } = new();
    public Mock<IUsuarioService> UsuarioServiceMock { get; } = new();
    public Mock<IClienteContactoService> ClienteContactoServiceMock { get; } = new();
    public Mock<IContactoService> ContactoServiceMock { get; } = new();
    public Mock<IDomicilioService> DomicilioServiceMock { get; } = new();
    public Mock<IEmailService> EmailServiceMock { get; } = new();
    public Mock<IListaPrecioService> ListaPrecioServiceMock { get; } = new();
    public Mock<IMovimientoStockService> MovimientoStockServiceMock { get; } = new();
    public Mock<IMovimientoStockDetalleService> MovimientoStockDetalleServiceMock { get; } = new();
    public Mock<IStockService> StockServiceMock { get; } = new();
    public Mock<ITelefonoService> TelefonoServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped(_ => AuthServiceMock.Object);
            services.AddScoped(_ => ProvinciaServiceMock.Object);
            services.AddScoped(_ => SubCategoriaServiceMock.Object);
            services.AddScoped(_ => AlicuotaImpuestoServiceMock.Object);
            services.AddScoped(_ => CategoriaServiceMock.Object);
            services.AddScoped(_ => CondicionIvaServiceMock.Object);
            services.AddScoped(_ => CondicionPagoServiceMock.Object);
            services.AddScoped(_ => DepositoServiceMock.Object);
            services.AddScoped(_ => EstadoCajaServiceMock.Object);
            services.AddScoped(_ => EstadoCompraServiceMock.Object);
            services.AddScoped(_ => EstadoFacturaServiceMock.Object);
            services.AddScoped(_ => EstadoPedidoServiceMock.Object);
            services.AddScoped(_ => EstadoProcesoServiceMock.Object);
            services.AddScoped(_ => MedioPagoServiceMock.Object);
            services.AddScoped(_ => MonedaServiceMock.Object);
            services.AddScoped(_ => MotivoAnulacionServiceMock.Object);
            services.AddScoped(_ => TipoComprobanteServiceMock.Object);
            services.AddScoped(_ => TipoImpuestoServiceMock.Object);
            services.AddScoped(_ => TipoMovimientoCajaServiceMock.Object);
            services.AddScoped(_ => TipoMovimientoStockServiceMock.Object);
            services.AddScoped(_ => TipoUsuarioServiceMock.Object);
            services.AddScoped(_ => CajaCabeceraServiceMock.Object);
            services.AddScoped(_ => CajaDetalleServiceMock.Object);
            services.AddScoped(_ => ClienteServiceMock.Object);
            services.AddScoped(_ => ProveedorServiceMock.Object);
            services.AddScoped(_ => ProductoServiceMock.Object);
            services.AddScoped(_ => UsuarioServiceMock.Object);
            services.AddScoped(_ => ClienteContactoServiceMock.Object);
            services.AddScoped(_ => ContactoServiceMock.Object);
            services.AddScoped(_ => DomicilioServiceMock.Object);
            services.AddScoped(_ => EmailServiceMock.Object);
            services.AddScoped(_ => ListaPrecioServiceMock.Object);
            services.AddScoped(_ => MovimientoStockServiceMock.Object);
            services.AddScoped(_ => MovimientoStockDetalleServiceMock.Object);
            services.AddScoped(_ => StockServiceMock.Object);
            services.AddScoped(_ => TelefonoServiceMock.Object);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
        });
    }
}
