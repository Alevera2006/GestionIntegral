using AutoMapper;
using GestionIntegral.Application.DTOs.AlicuotasImpuesto;
using GestionIntegral.Application.DTOs.Cajas;
using GestionIntegral.Application.DTOs.Categorias;
using GestionIntegral.Application.DTOs.CuentasCorrientes;
using GestionIntegral.Application.DTOs.Facturacion;
using GestionIntegral.Application.DTOs.PagosProveedores;
using GestionIntegral.Application.DTOs.ValoresCartera;
using GestionIntegral.Application.DTOs.ClienteContactos;
using GestionIntegral.Application.DTOs.CondicionesIVA;
using GestionIntegral.Application.DTOs.Contactos;
using GestionIntegral.Application.DTOs.Depositos;
using GestionIntegral.Application.DTOs.EstadosCaja;
using GestionIntegral.Application.DTOs.MediosPago;
using GestionIntegral.Application.DTOs.Monedas;
using GestionIntegral.Application.DTOs.MovimientosStock;
using GestionIntegral.Application.DTOs.Stock;
using GestionIntegral.Application.DTOs.TiposMovimientoCaja;
using GestionIntegral.Application.DTOs.TiposMovimientoStock;
using GestionIntegral.Application.DTOs.CondicionesPago;
using GestionIntegral.Application.DTOs.Clientes;
using GestionIntegral.Application.DTOs.EstadosCompra;
using GestionIntegral.Application.DTOs.EstadosFactura;
using GestionIntegral.Application.DTOs.EstadosPedido;
using GestionIntegral.Application.DTOs.EstadosProceso;
using GestionIntegral.Application.DTOs.ListaPrecioDetalles;
using GestionIntegral.Application.DTOs.ListasPrecios;
using GestionIntegral.Application.DTOs.MotivosAnulacion;
using GestionIntegral.Application.DTOs.Productos;
using GestionIntegral.Application.DTOs.Proveedores;
using GestionIntegral.Application.DTOs.Provincias;
using GestionIntegral.Application.DTOs.SubCategorias;
using GestionIntegral.Application.DTOs.TiposComprobante;
using GestionIntegral.Application.DTOs.TiposImpuesto;
using GestionIntegral.Application.DTOs.TiposUsuario;
using GestionIntegral.Application.DTOs.Usuarios;
using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TipoUsuario, TipoUsuarioResponseDto>();

        CreateMap<Usuario, UsuarioResponseDto>()
            .ForMember(dest => dest.TipoUsuarioDescripcion,
                       opt => opt.MapFrom(src => src.TipoUsuario.Descripcion));

        CreateMap<CondicionPago, CondicionPagoResponseDto>();

        CreateMap<Cliente, ClienteResponseDto>()
            .ForMember(dest => dest.CondicionPagoDescripcion,
                       opt => opt.MapFrom(src => src.CondicionPago != null ? src.CondicionPago.Descripcion : null));

        CreateMap<Proveedor, ProveedorResponseDto>()
            .ForMember(dest => dest.CondicionPagoDescripcion,
                       opt => opt.MapFrom(src => src.CondicionPago != null ? src.CondicionPago.Descripcion : null));

        CreateMap<Categoria, CategoriaResponseDto>();

        CreateMap<SubCategoria, SubCategoriaResponseDto>()
            .ForMember(dest => dest.CategoriaDescripcion,
                       opt => opt.MapFrom(src => src.Categoria.Descripcion));

        CreateMap<Producto, ProductoResponseDto>()
            .ForMember(dest => dest.CategoriaDescripcion,
                       opt => opt.MapFrom(src => src.Categoria.Descripcion))
            .ForMember(dest => dest.SubCategoriaDescripcion,
                       opt => opt.MapFrom(src => src.SubCategoria != null ? src.SubCategoria.Descripcion : null));

        CreateMap<ListaPrecio, ListaPrecioResponseDto>();

        CreateMap<ListaPrecioDetalle, ListaPrecioDetalleResponseDto>()
            .ForMember(dest => dest.ProductoCodigo,
                       opt => opt.MapFrom(src => src.Producto.Codigo))
            .ForMember(dest => dest.ProductoDescripcion,
                       opt => opt.MapFrom(src => src.Producto.Descripcion));

        CreateMap<TipoImpuesto, TipoImpuestoResponseDto>();

        CreateMap<AlicuotaImpuesto, AlicuotaImpuestoResponseDto>()
            .ForMember(dest => dest.TipoImpuestoDescripcion,
                       opt => opt.MapFrom(src => src.TipoImpuesto.Descripcion));

        CreateMap<CondicionIVA, CondicionIVAResponseDto>();

        CreateMap<Provincia, ProvinciaResponseDto>();
        CreateMap<TipoComprobante, TipoComprobanteResponseDto>();
        CreateMap<MotivoAnulacion, MotivoAnulacionResponseDto>();
        CreateMap<EstadoPedido, EstadoPedidoResponseDto>();
        CreateMap<EstadoFactura, EstadoFacturaResponseDto>();
        CreateMap<EstadoCompra, EstadoCompraResponseDto>();
        CreateMap<EstadoProceso, EstadoProcesoResponseDto>();

        CreateMap<Moneda, MonedaResponseDto>();
        CreateMap<MedioPago, MedioPagoResponseDto>();
        CreateMap<TipoMovimientoCaja, TipoMovimientoCajaResponseDto>();
        CreateMap<EstadoCaja, EstadoCajaResponseDto>();

        CreateMap<CajaCabecera, CajaCabeceraResponseDto>()
            .ForMember(dest => dest.NombreUsuarioResponsable,
                       opt => opt.MapFrom(src => $"{src.UsuarioResponsable.Nombre} {src.UsuarioResponsable.Apellido}"))
            .ForMember(dest => dest.EstadoCajaDescripcion,
                       opt => opt.MapFrom(src => src.EstadoCaja.Descripcion));

        CreateMap<CajaDetalle, CajaDetalleResponseDto>()
            .ForMember(dest => dest.TipoMovimientoCajaDescripcion,
                       opt => opt.MapFrom(src => src.TipoMovimientoCaja.Descripcion))
            .ForMember(dest => dest.MedioPagoDescripcion,
                       opt => opt.MapFrom(src => src.MedioPago != null ? src.MedioPago.Descripcion : null))
            .ForMember(dest => dest.MonedaCodigo,
                       opt => opt.MapFrom(src => src.Moneda != null ? src.Moneda.Codigo : null));

        CreateMap<Deposito, DepositoResponseDto>();
        CreateMap<TipoMovimientoStock, TipoMovimientoStockResponseDto>();

        CreateMap<Stock, StockResponseDto>()
            .ForMember(dest => dest.DepositoDescripcion,
                       opt => opt.MapFrom(src => src.Deposito.Descripcion))
            .ForMember(dest => dest.ProductoCodigo,
                       opt => opt.MapFrom(src => src.Producto.Codigo))
            .ForMember(dest => dest.ProductoDescripcion,
                       opt => opt.MapFrom(src => src.Producto.Descripcion));

        CreateMap<MovimientoStock, MovimientoStockResponseDto>()
            .ForMember(dest => dest.DepositoDescripcion,
                       opt => opt.MapFrom(src => src.Deposito.Descripcion))
            .ForMember(dest => dest.TipoMovimientoStockDescripcion,
                       opt => opt.MapFrom(src => src.TipoMovimientoStock.Descripcion))
            .ForMember(dest => dest.NombreUsuarioResponsable,
                       opt => opt.MapFrom(src => $"{src.UsuarioResponsable.Nombre} {src.UsuarioResponsable.Apellido}"));

        CreateMap<MovimientoStockDetalle, MovimientoStockDetalleResponseDto>()
            .ForMember(dest => dest.ProductoCodigo,
                       opt => opt.MapFrom(src => src.Producto.Codigo))
            .ForMember(dest => dest.ProductoDescripcion,
                       opt => opt.MapFrom(src => src.Producto.Descripcion));

        CreateMap<MovimientoStockDetalle, HistorialStockPorProductoResponseDto>()
            .ForMember(dest => dest.IdMovimientoStock,
                       opt => opt.MapFrom(src => src.MovimientoStock.IdMovimientoStock))
            .ForMember(dest => dest.FechaMovimiento,
                       opt => opt.MapFrom(src => src.MovimientoStock.FechaMovimiento))
            .ForMember(dest => dest.IdDeposito,
                       opt => opt.MapFrom(src => src.MovimientoStock.IdDeposito))
            .ForMember(dest => dest.DepositoDescripcion,
                       opt => opt.MapFrom(src => src.MovimientoStock.Deposito.Descripcion))
            .ForMember(dest => dest.IdTipoMovimientoStock,
                       opt => opt.MapFrom(src => src.MovimientoStock.IdTipoMovimientoStock))
            .ForMember(dest => dest.TipoMovimientoStockDescripcion,
                       opt => opt.MapFrom(src => src.MovimientoStock.TipoMovimientoStock.Descripcion))
            .ForMember(dest => dest.Signo,
                       opt => opt.MapFrom(src => src.MovimientoStock.TipoMovimientoStock.Signo))
            .ForMember(dest => dest.IdUsuarioResponsable,
                       opt => opt.MapFrom(src => src.MovimientoStock.IdUsuarioResponsable))
            .ForMember(dest => dest.NombreUsuarioResponsable,
                       opt => opt.MapFrom(src => $"{src.MovimientoStock.UsuarioResponsable.Nombre} {src.MovimientoStock.UsuarioResponsable.Apellido}"))
            .ForMember(dest => dest.Referencia,
                       opt => opt.MapFrom(src => src.MovimientoStock.Referencia))
            .ForMember(dest => dest.Observaciones,
                       opt => opt.MapFrom(src => src.MovimientoStock.Observaciones));

        CreateMap<Contacto, ContactoResponseDto>();

        CreateMap<ClienteContacto, ClienteContactoResponseDto>()
            .ForMember(dest => dest.ClienteRazonSocial,
                       opt => opt.MapFrom(src => src.Cliente.RazonSocial))
            .ForMember(dest => dest.ContactoNombre,
                       opt => opt.MapFrom(src => $"{src.Contacto.Nombre} {src.Contacto.Apellido}"));

        CreateMap<Factura, FacturaResponseDto>()
            .ForMember(dest => dest.ClienteNombre,
                       opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.RazonSocial : src.NombreCliente))
            .ForMember(dest => dest.TipoComprobante,
                       opt => opt.MapFrom(src => src.TipoComprobante.Descripcion))
            .ForMember(dest => dest.CondicionPago,
                       opt => opt.MapFrom(src => src.CondicionPago.Descripcion))
            .ForMember(dest => dest.EsCuentaCorriente,
                       opt => opt.MapFrom(src => src.CondicionPago.Descripcion.ToLower().Contains("cuenta corriente")));

        CreateMap<FacturaDetalle, FacturaDetalleResponseDto>()
            .ForMember(dest => dest.ProductoDescripcion,
                       opt => opt.MapFrom(src => src.Producto != null ? src.Producto.Descripcion : src.Descripcion));

        CreateMap<ValorCartera, ValorCarteraResponseDto>()
            .ForMember(dest => dest.ClienteNombre,
                       opt => opt.MapFrom(src => src.Cliente.RazonSocial));

        CreateMap<PagoProveedor, PagoProveedorResponseDto>()
            .ForMember(dest => dest.ProveedorNombre,
                       opt => opt.MapFrom(src => src.Proveedor.RazonSocial))
            .ForMember(dest => dest.MedioPago,
                       opt => opt.MapFrom(src => src.MedioPago.Descripcion));
    }
}
