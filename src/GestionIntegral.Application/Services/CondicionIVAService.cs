using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.CondicionesIVA;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class CondicionIVAService : ICondicionIVAService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CondicionIVAService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<CondicionIVAResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var condiciones = await _unitOfWork.CondicionesIVA.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<CondicionIVAResponseDto>>.Ok(_mapper.Map<IEnumerable<CondicionIVAResponseDto>>(condiciones));
    }

    public async Task<BaseResponseDto<CondicionIVAResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var condicion = await _unitOfWork.CondicionesIVA.GetByIdAsync(id, cancellationToken);
        if (condicion is null)
            throw new NotFoundException(nameof(CondicionIVA), id);

        return BaseResponseDto<CondicionIVAResponseDto>.Ok(_mapper.Map<CondicionIVAResponseDto>(condicion));
    }

    public async Task<BaseResponseDto<CondicionIVAResponseDto>> CrearAsync(CrearCondicionIVARequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.CondicionesIVA.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe una condición de IVA con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var condicion = new CondicionIVA
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.CondicionesIVA.AddAsync(condicion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<CondicionIVAResponseDto>.Ok(_mapper.Map<CondicionIVAResponseDto>(condicion), "Condición de IVA creada correctamente.");
    }

    public async Task<BaseResponseDto<CondicionIVAResponseDto>> ActualizarAsync(ActualizarCondicionIVARequestDto request, CancellationToken cancellationToken = default)
    {
        var condicion = await _unitOfWork.CondicionesIVA.GetByIdAsync(request.IdCondicionIVA, cancellationToken);
        if (condicion is null)
            throw new NotFoundException(nameof(CondicionIVA), request.IdCondicionIVA);

        var existente = await _unitOfWork.CondicionesIVA.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdCondicionIVA != request.IdCondicionIVA)
            throw new BusinessException($"Ya existe una condición de IVA con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        condicion.Codigo = request.Codigo;
        condicion.Descripcion = request.Descripcion;
        condicion.Activo = request.Activo;
        condicion.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        condicion.IdUsuarioUltimaModificacion = userId;
        condicion.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.CondicionesIVA.UpdateAsync(condicion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<CondicionIVAResponseDto>.Ok(_mapper.Map<CondicionIVAResponseDto>(condicion), "Condición de IVA actualizada correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var condicion = await _unitOfWork.CondicionesIVA.GetByIdAsync(id, cancellationToken);
        if (condicion is null)
            throw new NotFoundException(nameof(CondicionIVA), id);

        if (!condicion.Activo)
            throw new BusinessException("La condición de IVA ya se encuentra dada de baja.");

        var userId = _currentUserService.UserId ?? 1;

        condicion.Activo = false;
        condicion.FechaBaja = DateTime.UtcNow;
        condicion.IdUsuarioUltimaModificacion = userId;
        condicion.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.CondicionesIVA.UpdateAsync(condicion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Condición de IVA dada de baja correctamente.");
    }
}
