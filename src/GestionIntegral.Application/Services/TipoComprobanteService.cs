using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.TiposComprobante;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class TipoComprobanteService : ITipoComprobanteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public TipoComprobanteService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<TipoComprobanteResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var tipos = await _unitOfWork.TiposComprobante.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<TipoComprobanteResponseDto>>.Ok(_mapper.Map<IEnumerable<TipoComprobanteResponseDto>>(tipos));
    }

    public async Task<BaseResponseDto<TipoComprobanteResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposComprobante.GetByIdAsync(id, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoComprobante), id);

        return BaseResponseDto<TipoComprobanteResponseDto>.Ok(_mapper.Map<TipoComprobanteResponseDto>(tipo));
    }

    public async Task<BaseResponseDto<TipoComprobanteResponseDto>> CrearAsync(CrearTipoComprobanteRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.TiposComprobante.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un tipo de comprobante con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        var tipo = new TipoComprobante
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            CodigoAFIP = request.CodigoAFIP,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.TiposComprobante.AddAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<TipoComprobanteResponseDto>.Ok(_mapper.Map<TipoComprobanteResponseDto>(tipo), "Tipo de comprobante creado correctamente.");
    }

    public async Task<BaseResponseDto<TipoComprobanteResponseDto>> ActualizarAsync(ActualizarTipoComprobanteRequestDto request, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposComprobante.GetByIdAsync(request.IdTipoComprobante, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoComprobante), request.IdTipoComprobante);

        var existente = await _unitOfWork.TiposComprobante.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdTipoComprobante != request.IdTipoComprobante)
            throw new BusinessException($"Ya existe un tipo de comprobante con el código '{request.Codigo}'.");

        var userId = _currentUserService.UserId ?? 1;

        tipo.Codigo = request.Codigo;
        tipo.Descripcion = request.Descripcion;
        tipo.CodigoAFIP = request.CodigoAFIP;
        tipo.Activo = request.Activo;
        tipo.FechaBaja = request.Activo ? null : DateTime.UtcNow;
        tipo.IdUsuarioUltimaModificacion = userId;
        tipo.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.TiposComprobante.UpdateAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<TipoComprobanteResponseDto>.Ok(_mapper.Map<TipoComprobanteResponseDto>(tipo), "Tipo de comprobante actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposComprobante.GetByIdAsync(id, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoComprobante), id);

        if (!tipo.Activo)
            throw new BusinessException("El tipo de comprobante ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        tipo.Activo = false;
        tipo.FechaBaja = DateTime.UtcNow;
        tipo.IdUsuarioUltimaModificacion = userId;
        tipo.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.TiposComprobante.UpdateAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Tipo de comprobante dado de baja correctamente.");
    }
}
