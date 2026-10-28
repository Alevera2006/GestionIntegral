using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.MotivosAnulacion;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class MotivoAnulacionService : IMotivoAnulacionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public MotivoAnulacionService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<MotivoAnulacionResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var motivos = await _unitOfWork.MotivosAnulacion.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<MotivoAnulacionResponseDto>>.Ok(_mapper.Map<IEnumerable<MotivoAnulacionResponseDto>>(motivos));
    }

    public async Task<BaseResponseDto<MotivoAnulacionResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var motivo = await _unitOfWork.MotivosAnulacion.GetByIdAsync(id, cancellationToken);
        if (motivo is null)
            throw new NotFoundException(nameof(MotivoAnulacion), id);

        return BaseResponseDto<MotivoAnulacionResponseDto>.Ok(_mapper.Map<MotivoAnulacionResponseDto>(motivo));
    }

    public async Task<BaseResponseDto<MotivoAnulacionResponseDto>> CrearAsync(CrearMotivoAnulacionRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.MotivosAnulacion.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un motivo de anulación con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var motivo = new MotivoAnulacion
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.MotivosAnulacion.AddAsync(motivo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<MotivoAnulacionResponseDto>.Ok(_mapper.Map<MotivoAnulacionResponseDto>(motivo), "Motivo de anulación creado correctamente.");
    }

    public async Task<BaseResponseDto<MotivoAnulacionResponseDto>> ActualizarAsync(ActualizarMotivoAnulacionRequestDto request, CancellationToken cancellationToken = default)
    {
        var motivo = await _unitOfWork.MotivosAnulacion.GetByIdAsync(request.IdMotivoAnulacion, cancellationToken);
        if (motivo is null)
            throw new NotFoundException(nameof(MotivoAnulacion), request.IdMotivoAnulacion);

        var existente = await _unitOfWork.MotivosAnulacion.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdMotivoAnulacion != request.IdMotivoAnulacion)
            throw new BusinessException($"Ya existe un motivo de anulación con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        motivo.Codigo = request.Codigo;
        motivo.Descripcion = request.Descripcion;
        motivo.Activo = request.Activo;
        motivo.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        motivo.IdUsuarioUltimaModificacion = userId;
        motivo.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.MotivosAnulacion.UpdateAsync(motivo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<MotivoAnulacionResponseDto>.Ok(_mapper.Map<MotivoAnulacionResponseDto>(motivo), "Motivo de anulación actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var motivo = await _unitOfWork.MotivosAnulacion.GetByIdAsync(id, cancellationToken);
        if (motivo is null)
            throw new NotFoundException(nameof(MotivoAnulacion), id);

        if (!motivo.Activo)
            throw new BusinessException("El motivo de anulación ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        motivo.Activo = false;
        motivo.FechaBaja = DateTime.UtcNow;
        motivo.IdUsuarioUltimaModificacion = userId;
        motivo.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.MotivosAnulacion.UpdateAsync(motivo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Motivo de anulación dado de baja correctamente.");
    }
}
