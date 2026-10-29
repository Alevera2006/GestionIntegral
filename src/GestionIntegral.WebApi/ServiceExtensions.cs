using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Application.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GestionIntegral.WebApi;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<MappingProfile>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();
        services.AddScoped<ICondicionPagoService, CondicionPagoService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IProveedorService, ProveedorService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<ISubCategoriaService, SubCategoriaService>();
        services.AddScoped<IProductoService, ProductoService>();
        services.AddScoped<IListaPrecioService, ListaPrecioService>();
        services.AddScoped<ITipoImpuestoService, TipoImpuestoService>();
        services.AddScoped<IAlicuotaImpuestoService, AlicuotaImpuestoService>();
        services.AddScoped<ICondicionIVAService, CondicionIVAService>();
        services.AddScoped<IProvinciaService, ProvinciaService>();
        services.AddScoped<ITipoComprobanteService, TipoComprobanteService>();
        services.AddScoped<IMotivoAnulacionService, MotivoAnulacionService>();
        services.AddScoped<IEstadoPedidoService, EstadoPedidoService>();
        services.AddScoped<IEstadoFacturaService, EstadoFacturaService>();
        services.AddScoped<IEstadoCompraService, EstadoCompraService>();
        services.AddScoped<IEstadoProcesoService, EstadoProcesoService>();
        services.AddScoped<IMonedaService, MonedaService>();
        services.AddScoped<IMedioPagoService, MedioPagoService>();
        services.AddScoped<ITipoMovimientoCajaService, TipoMovimientoCajaService>();
        services.AddScoped<IEstadoCajaService, EstadoCajaService>();
        services.AddScoped<ICajaCabeceraService, CajaCabeceraService>();
        services.AddScoped<ICajaDetalleService, CajaDetalleService>();
        services.AddScoped<IDepositoService, DepositoService>();
        services.AddScoped<ITipoMovimientoStockService, TipoMovimientoStockService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IMovimientoStockService, MovimientoStockService>();
        services.AddScoped<IMovimientoStockDetalleService, MovimientoStockDetalleService>();
        services.AddScoped<IContactoService, ContactoService>();
        services.AddScoped<IClienteContactoService, ClienteContactoService>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
        ArgumentException.ThrowIfNullOrEmpty(jwtSettings?.Secret);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestionIntegral API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
