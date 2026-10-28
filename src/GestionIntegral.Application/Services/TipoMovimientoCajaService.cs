using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.TiposMovimientoCaja;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class TipoMovimientoCajaService : ITipoMovimientoCajaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public TipoMovimientoCajaService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<TipoMovimientoCajaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var tipos = await _unitOfWork.TiposMovimientoCaja.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<TipoMovimientoCajaResponseDto>>.Ok(_mapper.Map<IEnumerable<TipoMovimientoCajaResponseDto>>(tipos));
    }

    public async Task<BaseResponseDto<TipoMovimientoCajaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposMovimientoCaja.GetByIdAsync(id, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoMovimientoCaja), id);

        return BaseResponseDto<TipoMovimientoCajaResponseDto>.Ok(_mapper.Map<TipoMovimientoCajaResponseDto>(tipo));
    }

    public async Task<BaseResponseDto<TipoMovimientoCajaResponseDto>> CrearAsync(CrearTipoMovimientoCajaRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.TiposMovimientoCaja.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un tipo de movimiento de caja con el código '{request.Codigo}'.");

        if (request.Signo != -1 && request.Signo != 1)
            throw new BusinessException("El signo debe ser -1 o 1.");

        var userId = _currentUserService.UserId ?? 1;

        var tipo = new TipoMovimientoCaja
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Signo = request.Signo,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.TiposMovimientoCaja.AddAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<TipoMovimientoCajaResponseDto>.Ok(_mapper.Map<TipoMovimientoCajaResponseDto>(tipo), "Tipo de movimiento de caja creado correctamente.");
    }

    public async Task<BaseResponseDto<TipoMovimientoCajaResponseDto>> ActualizarAsync(ActualizarTipoMovimientoCajaRequestDto request, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposMovimientoCaja.GetByIdAsync(request.IdTipoMovimientoCaja, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoMovimientoCaja), request.IdTipoMovimientoCaja);

        var existente = await _unitOfWork.TiposMovimientoCaja.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdTipoMovimientoCaja != request.IdTipoMovimientoCaja)
            throw new BusinessException($"Ya existe un tipo de movimiento de caja con el código '{request.Codigo}'.");

        if (request.Signo != -1 && request.Signo != 1)
            throw new BusinessException("El signo debe ser -1 o 1.");

        var userId = _currentUserService.UserId ?? 1;

        tipo.Codigo = request.Codigo;
        tipo.Descripcion = request.Descripcion;
        tipo.Signo = request.Signo;
        tipo.Activo = request.Activo;
        tipo.IdUsuarioUltimaModificacion = userId;
        tipo.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.TiposMovimientoCaja.UpdateAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<TipoMovimientoCajaResponseDto>.Ok(_mapper.Map<TipoMovimientoCajaResponseDto>(tipo), "Tipo de movimiento de caja actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposMovimientoCaja.GetByIdAsync(id, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoMovimientoCaja), id);

        if (!tipo.Activo)
            throw new BusinessException("El tipo de movimiento de caja ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        tipo.Activo = false;
        tipo.IdUsuarioUltimaModificacion = userId;
        tipo.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.TiposMovimientoCaja.UpdateAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Tipo de movimiento de caja dado de baja correctamente.");
    }
}
