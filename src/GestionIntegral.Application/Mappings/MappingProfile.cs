using AutoMapper;
using GestionIntegral.Application.DTOs.AlicuotasImpuesto;
using GestionIntegral.Application.DTOs.Cajas;
using GestionIntegral.Application.DTOs.Categorias;
using GestionIntegral.Application.DTOs.CondicionesIVA;
using GestionIntegral.Application.DTOs.EstadosCaja;
using GestionIntegral.Application.DTOs.MediosPago;
using GestionIntegral.Application.DTOs.Monedas;
using GestionIntegral.Application.DTOs.TiposMovimientoCaja;
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
    }
}
