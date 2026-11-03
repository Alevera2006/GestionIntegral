using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Facturacion;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class FacturaService : IFacturaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public FacturaService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<FacturaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var facturas = await _unitOfWork.Facturas.GetAllWithDetailsAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<FacturaResponseDto>>.Ok(_mapper.Map<IEnumerable<FacturaResponseDto>>(facturas));
    }

    public async Task<BaseResponseDto<FacturaResponseDto>> CrearAsync(FacturaRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Facturas.ExisteNumeroComprobanteAsync(request.NumeroComprobante, cancellationToken))
            throw new BusinessException($"Ya existe una factura con el número '{request.NumeroComprobante}'.");

        var tipoComprobante = await _unitOfWork.TiposComprobante.GetByIdAsync(request.IdTipoComprobante, cancellationToken);
        if (tipoComprobante is null)
            throw new NotFoundException(nameof(TipoComprobante), request.IdTipoComprobante);

        var condicionPago = await _unitOfWork.CondicionesPago.GetByIdAsync(request.IdCondicionPago, cancellationToken);
        if (condicionPago is null)
            throw new NotFoundException(nameof(CondicionPago), request.IdCondicionPago);

        Cliente? cliente = null;
        if (request.IdCliente.HasValue)
        {
            cliente = await _unitOfWork.Clientes.GetByIdAsync(request.IdCliente.Value, cancellationToken);
            if (cliente is null)
                throw new NotFoundException(nameof(Cliente), request.IdCliente.Value);
        }

        if (request.Detalles is null || !request.Detalles.Any())
            throw new BusinessException("La factura debe tener al menos un ítem.");

        var factura = new Factura
        {
            IdCliente = request.IdCliente,
            NombreCliente = request.IdCliente.HasValue && cliente != null ? cliente.RazonSocial : (request.NombreCliente ?? "Consumidor Final"),
            IdTipoComprobante = request.IdTipoComprobante,
            NumeroComprobante = request.NumeroComprobante,
            Fecha = request.Fecha,
            IdCondicionPago = request.IdCondicionPago,
            Observaciones = request.Observaciones ?? string.Empty,
            Activo = true,
            IdUsuarioCreacion = _currentUserService.UserId ?? 1,
            FechaCreacion = DateTime.UtcNow
        };

        decimal subtotal = 0;
        decimal importeIva = 0;

        foreach (var item in request.Detalles)
        {
            var producto = await _unitOfWork.Productos.GetByIdAsync(item.IdProducto, cancellationToken);
            if (producto is null)
                throw new NotFoundException(nameof(Producto), item.IdProducto);

            var itemSubtotal = item.Cantidad * item.PrecioUnitario;
            var itemIva = itemSubtotal * (item.PorcentajeIva / 100m);

            factura.Detalles.Add(new FacturaDetalle
            {
                IdProducto = item.IdProducto,
                Descripcion = string.IsNullOrWhiteSpace(item.Descripcion) ? producto.Descripcion : item.Descripcion,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario,
                PorcentajeIva = item.PorcentajeIva,
                Subtotal = itemSubtotal,
                ImporteIva = itemIva
            });

            subtotal += itemSubtotal;
            importeIva += itemIva;
        }

        factura.Subtotal = subtotal;
        factura.ImporteIva = importeIva;
        factura.Total = subtotal + importeIva;

        await _unitOfWork.Facturas.AddAsync(factura, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<FacturaResponseDto>.Ok(_mapper.Map<FacturaResponseDto>(factura), "Factura registrada correctamente.");
    }
}
