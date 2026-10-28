using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.EstadosProceso;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class EstadoProcesoService : IEstadoProcesoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public EstadoProcesoService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<EstadoProcesoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var estados = await _unitOfWork.EstadosProceso.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<EstadoProcesoResponseDto>>.Ok(_mapper.Map<IEnumerable<EstadoProcesoResponseDto>>(estados));
    }

    public async Task<BaseResponseDto<EstadoProcesoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosProceso.GetByIdAsync(id, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoProceso), id);

        return BaseResponseDto<EstadoProcesoResponseDto>.Ok(_mapper.Map<EstadoProcesoResponseDto>(estado));
    }

    public async Task<BaseResponseDto<EstadoProcesoResponseDto>> CrearAsync(CrearEstadoProcesoRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.EstadosProceso.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un estado de proceso con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var estado = new EstadoProceso
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.EstadosProceso.AddAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<EstadoProcesoResponseDto>.Ok(_mapper.Map<EstadoProcesoResponseDto>(estado), "Estado de proceso creado correctamente.");
    }

    public async Task<BaseResponseDto<EstadoProcesoResponseDto>> ActualizarAsync(ActualizarEstadoProcesoRequestDto request, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosProceso.GetByIdAsync(request.IdEstadoProceso, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoProceso), request.IdEstadoProceso);

        var existente = await _unitOfWork.EstadosProceso.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdEstadoProceso != request.IdEstadoProceso)
            throw new BusinessException($"Ya existe un estado de proceso con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        estado.Codigo = request.Codigo;
        estado.Descripcion = request.Descripcion;
        estado.Activo = request.Activo;
        estado.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        estado.IdUsuarioUltimaModificacion = userId;
        estado.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.EstadosProceso.UpdateAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<EstadoProcesoResponseDto>.Ok(_mapper.Map<EstadoProcesoResponseDto>(estado), "Estado de proceso actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var estado = await _unitOfWork.EstadosProceso.GetByIdAsync(id, cancellationToken);
        if (estado is null)
            throw new NotFoundException(nameof(EstadoProceso), id);

        if (!estado.Activo)
            throw new BusinessException("El estado de proceso ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        estado.Activo = false;
        estado.FechaBaja = DateTime.UtcNow;
        estado.IdUsuarioUltimaModificacion = userId;
        estado.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.EstadosProceso.UpdateAsync(estado, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Estado de proceso dado de baja correctamente.");
    }
}
