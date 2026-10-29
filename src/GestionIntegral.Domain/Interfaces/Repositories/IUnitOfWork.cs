namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUsuarioRepository Usuarios { get; }
    ITipoUsuarioRepository TiposUsuario { get; }
    ICondicionPagoRepository CondicionesPago { get; }
    IClienteRepository Clientes { get; }
    IProveedorRepository Proveedores { get; }
    ICategoriaRepository Categorias { get; }
    ISubCategoriaRepository SubCategorias { get; }
    IProductoRepository Productos { get; }
    IListaPrecioRepository ListasPrecios { get; }
    IListaPrecioDetalleRepository ListaPrecioDetalles { get; }
    ITipoImpuestoRepository TiposImpuesto { get; }
    IAlicuotaImpuestoRepository AlicuotasImpuesto { get; }
    ICondicionIVARepository CondicionesIVA { get; }
    IProvinciaRepository Provincias { get; }
    ITipoComprobanteRepository TiposComprobante { get; }
    IMotivoAnulacionRepository MotivosAnulacion { get; }
    IEstadoPedidoRepository EstadosPedido { get; }
    IEstadoFacturaRepository EstadosFactura { get; }
    IEstadoCompraRepository EstadosCompra { get; }
    IEstadoProcesoRepository EstadosProceso { get; }
    IMonedaRepository Monedas { get; }
    IMedioPagoRepository MediosPago { get; }
    ITipoMovimientoCajaRepository TiposMovimientoCaja { get; }
    IEstadoCajaRepository EstadosCaja { get; }
    ICajaCabeceraRepository CajasCabecera { get; }
    ICajaDetalleRepository CajasDetalle { get; }
    IDepositoRepository Depositos { get; }
    ITipoMovimientoStockRepository TiposMovimientoStock { get; }
    IStockRepository Stock { get; }
    IMovimientoStockRepository MovimientosStock { get; }
    IMovimientoStockDetalleRepository MovimientoStockDetalle { get; }
    IContactoRepository Contactos { get; }
    IClienteContactoRepository ClienteContactos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
