using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Data;

public class GestionIntegralDbContext : DbContext
{
    public GestionIntegralDbContext(DbContextOptions<GestionIntegralDbContext> options) : base(options) { }

    public DbSet<TipoUsuario> TiposUsuario => Set<TipoUsuario>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<CondicionPago> CondicionesPago => Set<CondicionPago>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<SubCategoria> SubCategorias => Set<SubCategoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<ListaPrecio> ListasPrecios => Set<ListaPrecio>();
    public DbSet<ListaPrecioDetalle> ListaPreciosDetalle => Set<ListaPrecioDetalle>();
    public DbSet<TipoImpuesto> TiposImpuesto => Set<TipoImpuesto>();
    public DbSet<AlicuotaImpuesto> AlicuotasImpuesto => Set<AlicuotaImpuesto>();
    public DbSet<CondicionIVA> CondicionesIVA => Set<CondicionIVA>();
    public DbSet<Provincia> Provincias => Set<Provincia>();
    public DbSet<TipoComprobante> TiposComprobante => Set<TipoComprobante>();
    public DbSet<MotivoAnulacion> MotivosAnulacion => Set<MotivoAnulacion>();
    public DbSet<EstadoPedido> EstadosPedido => Set<EstadoPedido>();
    public DbSet<EstadoFactura> EstadosFactura => Set<EstadoFactura>();
    public DbSet<EstadoCompra> EstadosCompra => Set<EstadoCompra>();
    public DbSet<EstadoProceso> EstadosProceso => Set<EstadoProceso>();
    public DbSet<Moneda> Monedas => Set<Moneda>();
    public DbSet<MedioPago> MediosPago => Set<MedioPago>();
    public DbSet<TipoMovimientoCaja> TiposMovimientoCaja => Set<TipoMovimientoCaja>();
    public DbSet<EstadoCaja> EstadosCaja => Set<EstadoCaja>();
    public DbSet<CajaCabecera> CajasCabecera => Set<CajaCabecera>();
    public DbSet<CajaDetalle> CajasDetalle => Set<CajaDetalle>();
    public DbSet<Deposito> Depositos => Set<Deposito>();
    public DbSet<TipoMovimientoStock> TiposMovimientoStock => Set<TipoMovimientoStock>();
    public DbSet<Stock> Stock => Set<Stock>();
    public DbSet<MovimientoStock> MovimientosStock => Set<MovimientoStock>();
    public DbSet<MovimientoStockDetalle> MovimientoStockDetalle => Set<MovimientoStockDetalle>();
    public DbSet<Contacto> Contactos => Set<Contacto>();
    public DbSet<ClienteContacto> ClienteContactos => Set<ClienteContacto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GestionIntegralDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
