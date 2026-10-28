using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Cajas;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class CajaCabeceraService : ICajaCabeceraService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CajaCabeceraService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<CajaCabeceraResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var cajas = await _unitOfWork.CajasCabecera.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<CajaCabeceraResponseDto>>.Ok(_mapper.Map<IEnumerable<CajaCabeceraResponseDto>>(cajas));
    }

    public async Task<BaseResponseDto<CajaCabeceraResponseDto>> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var caja = await _unitOfWork.CajasCabecera.GetByIdAsync((int)id, cancellationToken);
        if (caja is null)
            throw new NotFoundException(nameof(CajaCabecera), id);

        return BaseResponseDto<CajaCabeceraResponseDto>.Ok(_mapper.Map<CajaCabeceraResponseDto>(caja));
    }

    public async Task<BaseResponseDto<CajaCabeceraResponseDto>> CrearAsync(CrearCajaCabeceraRequestDto request, CancellationToken cancellationToken = default)
    {
        var responsable = await _unitOfWork.Usuarios.GetByIdAsync(request.IdUsuarioResponsable, cancellationToken);
        if (responsable is null)
            throw new NotFoundException(nameof(Usuario), request.IdUsuarioResponsable);

        var estado = await _unitOfWork.EstadosCaja.GetByIdAsync(request.IdEstadoCaja, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoCaja), request.IdEstadoCaja);

        var userId = _currentUserService.UserId ?? 1;

        var caja = new CajaCabecera
        {
            FechaCaja = request.FechaCaja,
            IdUsuarioResponsable = request.IdUsuarioResponsable,
            IdEstadoCaja = request.IdEstadoCaja,
            SaldoInicial = request.SaldoInicial,
            FechaApertura = DateTime.UtcNow,
            Observaciones = request.Observaciones,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.CajasCabecera.AddAsync(caja, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<CajaCabeceraResponseDto>.Ok(_mapper.Map<CajaCabeceraResponseDto>(caja), "Caja creada correctamente.");
    }

    public async Task<BaseResponseDto<CajaCabeceraResponseDto>> ActualizarAsync(ActualizarCajaCabeceraRequestDto request, CancellationToken cancellationToken = default)
    {
        var caja = await _unitOfWork.CajasCabecera.GetByIdAsync((int)request.IdCaja, cancellationToken);
        if (caja is null)
            throw new NotFoundException(nameof(CajaCabecera), request.IdCaja);

        var responsable = await _unitOfWork.Usuarios.GetByIdAsync(request.IdUsuarioResponsable, cancellationToken);
        if (responsable is null)
            throw new NotFoundException(nameof(Usuario), request.IdUsuarioResponsable);

        var estado = await _unitOfWork.EstadosCaja.GetByIdAsync(request.IdEstadoCaja, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoCaja), request.IdEstadoCaja);

        var userId = _currentUserService.UserId ?? 1;

        caja.IdUsuarioResponsable = request.IdUsuarioResponsable;
        caja.IdEstadoCaja = request.IdEstadoCaja;
        caja.SaldoFinal = request.SaldoFinal;
        caja.FechaCierre = request.FechaCierre;
        caja.Observaciones = request.Observaciones;
        caja.IdUsuarioUltimaModificacion = userId;
        caja.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.CajasCabecera.UpdateAsync(caja, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<CajaCabeceraResponseDto>.Ok(_mapper.Map<CajaCabeceraResponseDto>(caja), "Caja actualizada correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(long id, CancellationToken cancellationToken = default)
    {
        var caja = await _unitOfWork.CajasCabecera.GetByIdAsync((int)id, cancellationToken);
        if (caja is null)
            throw new NotFoundException(nameof(CajaCabecera), id);

        await _unitOfWork.CajasCabecera.DeleteAsync(caja, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Caja eliminada correctamente.");
    }
}
