using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Cajas;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class CajaDetalleService : ICajaDetalleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CajaDetalleService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<CajaDetalleResponseDto>>> ObtenerPorCajaAsync(long idCaja, CancellationToken cancellationToken = default)
    {
        var detalles = await _unitOfWork.CajasDetalle.FindAsync(d => d.IdCaja == idCaja, cancellationToken);
        return BaseResponseDto<IEnumerable<CajaDetalleResponseDto>>.Ok(_mapper.Map<IEnumerable<CajaDetalleResponseDto>>(detalles));
    }

    public async Task<BaseResponseDto<CajaDetalleResponseDto>> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var detalle = await _unitOfWork.CajasDetalle.GetByIdAsync((int)id, cancellationToken);
        if (detalle is null)
            throw new NotFoundException(nameof(CajaDetalle), id);

        return BaseResponseDto<CajaDetalleResponseDto>.Ok(_mapper.Map<CajaDetalleResponseDto>(detalle));
    }

    public async Task<BaseResponseDto<CajaDetalleResponseDto>> CrearAsync(CrearCajaDetalleRequestDto request, CancellationToken cancellationToken = default)
    {
        var caja = await _unitOfWork.CajasCabecera.GetByIdAsync((int)request.IdCaja, cancellationToken);
        if (caja is null)
            throw new NotFoundException(nameof(CajaCabecera), request.IdCaja);

        var tipo = await _unitOfWork.TiposMovimientoCaja.GetByIdAsync(request.IdTipoMovimientoCaja, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoMovimientoCaja), request.IdTipoMovimientoCaja);

        if (request.IdMedioPago.HasValue && await _unitOfWork.MediosPago.GetByIdAsync(request.IdMedioPago.Value, cancellationToken) is null)
            throw new NotFoundException(nameof(MedioPago), request.IdMedioPago.Value);

        if (request.IdMoneda.HasValue && await _unitOfWork.Monedas.GetByIdAsync(request.IdMoneda.Value, cancellationToken) is null)
            throw new NotFoundException(nameof(Moneda), request.IdMoneda.Value);

        if (request.Importe <= 0)
            throw new BusinessException("El importe debe ser mayor a cero.");

        var userId = _currentUserService.UserId ?? 1;

        var detalle = new CajaDetalle
        {
            IdCaja = request.IdCaja,
            IdTipoMovimientoCaja = request.IdTipoMovimientoCaja,
            IdMedioPago = request.IdMedioPago,
            IdMoneda = request.IdMoneda,
            FechaMovimiento = DateTime.UtcNow,
            Concepto = request.Concepto,
            Referencia = request.Referencia,
            Importe = request.Importe,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.CajasDetalle.AddAsync(detalle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<CajaDetalleResponseDto>.Ok(_mapper.Map<CajaDetalleResponseDto>(detalle), "Detalle de caja creado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(long id, CancellationToken cancellationToken = default)
    {
        var detalle = await _unitOfWork.CajasDetalle.GetByIdAsync((int)id, cancellationToken);
        if (detalle is null)
            throw new NotFoundException(nameof(CajaDetalle), id);

        await _unitOfWork.CajasDetalle.DeleteAsync(detalle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Detalle de caja eliminado correctamente.");
    }
}
