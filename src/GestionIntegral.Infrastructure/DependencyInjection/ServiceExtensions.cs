using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Application.Settings;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using GestionIntegral.Infrastructure.Data;
using GestionIntegral.Infrastructure.Identity;
using GestionIntegral.Infrastructure.Repositories;
using GestionIntegral.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionIntegral.Infrastructure.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        services.AddDbContext<GestionIntegralDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();
        services.AddScoped<ICondicionPagoRepository, CondicionPagoRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IProveedorRepository, ProveedorRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<ISubCategoriaRepository, SubCategoriaRepository>();
        services.AddScoped<IProductoRepository, ProductoRepository>();
        services.AddScoped<IListaPrecioRepository, ListaPrecioRepository>();
        services.AddScoped<IListaPrecioDetalleRepository, ListaPrecioDetalleRepository>();
        services.AddScoped<ITipoImpuestoRepository, TipoImpuestoRepository>();
        services.AddScoped<IAlicuotaImpuestoRepository, AlicuotaImpuestoRepository>();
        services.AddScoped<ICondicionIVARepository, CondicionIVARepository>();
        services.AddScoped<IProvinciaRepository, ProvinciaRepository>();
        services.AddScoped<ITipoComprobanteRepository, TipoComprobanteRepository>();
        services.AddScoped<IMotivoAnulacionRepository, MotivoAnulacionRepository>();
        services.AddScoped<IEstadoPedidoRepository, EstadoPedidoRepository>();
        services.AddScoped<IEstadoFacturaRepository, EstadoFacturaRepository>();
        services.AddScoped<IEstadoCompraRepository, EstadoCompraRepository>();
        services.AddScoped<IEstadoProcesoRepository, EstadoProcesoRepository>();
        services.AddScoped<IMonedaRepository, MonedaRepository>();
        services.AddScoped<IMedioPagoRepository, MedioPagoRepository>();
        services.AddScoped<ITipoMovimientoCajaRepository, TipoMovimientoCajaRepository>();
        services.AddScoped<IEstadoCajaRepository, EstadoCajaRepository>();
        services.AddScoped<ICajaCabeceraRepository, CajaCabeceraRepository>();
        services.AddScoped<ICajaDetalleRepository, CajaDetalleRepository>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        return services;
    }
}
