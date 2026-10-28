using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.TiposImpuesto;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class TipoImpuestoService : ITipoImpuestoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public TipoImpuestoService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<TipoImpuestoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var tipos = await _unitOfWork.TiposImpuesto.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<TipoImpuestoResponseDto>>.Ok(_mapper.Map<IEnumerable<TipoImpuestoResponseDto>>(tipos));
    }

    public async Task<BaseResponseDto<TipoImpuestoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposImpuesto.GetByIdAsync(id, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoImpuesto), id);

        return BaseResponseDto<TipoImpuestoResponseDto>.Ok(_mapper.Map<TipoImpuestoResponseDto>(tipo));
    }

    public async Task<BaseResponseDto<TipoImpuestoResponseDto>> CrearAsync(CrearTipoImpuestoRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.TiposImpuesto.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un tipo de impuesto con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var tipo = new TipoImpuesto
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.TiposImpuesto.AddAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<TipoImpuestoResponseDto>.Ok(_mapper.Map<TipoImpuestoResponseDto>(tipo), "Tipo de impuesto creado correctamente.");
    }

    public async Task<BaseResponseDto<TipoImpuestoResponseDto>> ActualizarAsync(ActualizarTipoImpuestoRequestDto request, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposImpuesto.GetByIdAsync(request.IdTipoImpuesto, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoImpuesto), request.IdTipoImpuesto);

        var existente = await _unitOfWork.TiposImpuesto.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdTipoImpuesto != request.IdTipoImpuesto)
            throw new BusinessException($"Ya existe un tipo de impuesto con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        tipo.Codigo = request.Codigo;
        tipo.Descripcion = request.Descripcion;
        tipo.Activo = request.Activo;
        tipo.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        tipo.IdUsuarioUltimaModificacion = userId;
        tipo.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.TiposImpuesto.UpdateAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<TipoImpuestoResponseDto>.Ok(_mapper.Map<TipoImpuestoResponseDto>(tipo), "Tipo de impuesto actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposImpuesto.GetByIdAsync(id, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoImpuesto), id);

        if (!tipo.Activo)
            throw new BusinessException("El tipo de impuesto ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        tipo.Activo = false;
        tipo.FechaBaja = DateTime.UtcNow;
        tipo.IdUsuarioUltimaModificacion = userId;
        tipo.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.TiposImpuesto.UpdateAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Tipo de impuesto dado de baja correctamente.");
    }
}
