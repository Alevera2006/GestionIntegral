using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.EstadosCaja;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class EstadoCajaService : IEstadoCajaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public EstadoCajaService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<EstadoCajaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var estados = await _unitOfWork.EstadosCaja.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<EstadoCajaResponseDto>>.Ok(_mapper.Map<IEnumerable<EstadoCajaResponseDto>>(estados));
    }

    public async Task<BaseResponseDto<EstadoCajaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosCaja.GetByIdAsync(id, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoCaja), id);

        return BaseResponseDto<EstadoCajaResponseDto>.Ok(_mapper.Map<EstadoCajaResponseDto>(estado));
    }

    public async Task<BaseResponseDto<EstadoCajaResponseDto>> CrearAsync(CrearEstadoCajaRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.EstadosCaja.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un estado de caja con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var estado = new EstadoCaja
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.EstadosCaja.AddAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<EstadoCajaResponseDto>.Ok(_mapper.Map<EstadoCajaResponseDto>(estado), "Estado de caja creado correctamente.");
    }

    public async Task<BaseResponseDto<EstadoCajaResponseDto>> ActualizarAsync(ActualizarEstadoCajaRequestDto request, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosCaja.GetByIdAsync(request.IdEstadoCaja, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoCaja), request.IdEstadoCaja);

        var existente = await _unitOfWork.EstadosCaja.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdEstadoCaja != request.IdEstadoCaja)
            throw new BusinessException($"Ya existe un estado de caja con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        estado.Codigo = request.Codigo;
        estado.Descripcion = request.Descripcion;
        estado.Activo = request.Activo;
        estado.IdUsuarioUltimaModificacion = userId;
        estado.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.EstadosCaja.UpdateAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<EstadoCajaResponseDto>.Ok(_mapper.Map<EstadoCajaResponseDto>(estado), "Estado de caja actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosCaja.GetByIdAsync(id, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoCaja), id);

        if (!estado.Activo)
            throw new BusinessException("El estado de caja ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        estado.Activo = false;
        estado.IdUsuarioUltimaModificacion = userId;
        estado.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.EstadosCaja.UpdateAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Estado de caja dado de baja correctamente.");
    }
}
