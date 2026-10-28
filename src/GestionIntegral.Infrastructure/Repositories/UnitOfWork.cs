using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GestionIntegralDbContext _context;

    public IUsuarioRepository Usuarios { get; }
    public ITipoUsuarioRepository TiposUsuario { get; }
    public ICondicionPagoRepository CondicionesPago { get; }
    public IClienteRepository Clientes { get; }
    public IProveedorRepository Proveedores { get; }
    public ICategoriaRepository Categorias { get; }
    public ISubCategoriaRepository SubCategorias { get; }
    public IProductoRepository Productos { get; }
    public IListaPrecioRepository ListasPrecios { get; }
    public IListaPrecioDetalleRepository ListaPrecioDetalles { get; }
    public ITipoImpuestoRepository TiposImpuesto { get; }
    public IAlicuotaImpuestoRepository AlicuotasImpuesto { get; }
    public ICondicionIVARepository CondicionesIVA { get; }
    public IProvinciaRepository Provincias { get; }
    public ITipoComprobanteRepository TiposComprobante { get; }
    public IMotivoAnulacionRepository MotivosAnulacion { get; }
    public IEstadoPedidoRepository EstadosPedido { get; }
    public IEstadoFacturaRepository EstadosFactura { get; }
    public IEstadoCompraRepository EstadosCompra { get; }
    public IEstadoProcesoRepository EstadosProceso { get; }
    public IMonedaRepository Monedas { get; }
    public IMedioPagoRepository MediosPago { get; }
    public ITipoMovimientoCajaRepository TiposMovimientoCaja { get; }
    public IEstadoCajaRepository EstadosCaja { get; }
    public ICajaCabeceraRepository CajasCabecera { get; }
    public ICajaDetalleRepository CajasDetalle { get; }

    public UnitOfWork(GestionIntegralDbContext context)
    {
        _context = context;
        Usuarios = new UsuarioRepository(context);
        TiposUsuario = new TipoUsuarioRepository(context);
        CondicionesPago = new CondicionPagoRepository(context);
        Clientes = new ClienteRepository(context);
        Proveedores = new ProveedorRepository(context);
        Categorias = new CategoriaRepository(context);
        SubCategorias = new SubCategoriaRepository(context);
        Productos = new ProductoRepository(context);
        ListasPrecios = new ListaPrecioRepository(context);
        ListaPrecioDetalles = new ListaPrecioDetalleRepository(context);
        TiposImpuesto = new TipoImpuestoRepository(context);
        AlicuotasImpuesto = new AlicuotaImpuestoRepository(context);
        CondicionesIVA = new CondicionIVARepository(context);
        Provincias = new ProvinciaRepository(context);
        TiposComprobante = new TipoComprobanteRepository(context);
        MotivosAnulacion = new MotivoAnulacionRepository(context);
        EstadosPedido = new EstadoPedidoRepository(context);
        EstadosFactura = new EstadoFacturaRepository(context);
        EstadosCompra = new EstadoCompraRepository(context);
        EstadosProceso = new EstadoProcesoRepository(context);
        Monedas = new MonedaRepository(context);
        MediosPago = new MedioPagoRepository(context);
        TiposMovimientoCaja = new TipoMovimientoCajaRepository(context);
        EstadosCaja = new EstadoCajaRepository(context);
        CajasCabecera = new CajaCabeceraRepository(context);
        CajasDetalle = new CajaDetalleRepository(context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
