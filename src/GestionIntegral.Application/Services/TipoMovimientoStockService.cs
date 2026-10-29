using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.TiposMovimientoStock;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class TipoMovimientoStockService : ITipoMovimientoStockService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public TipoMovimientoStockService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<TipoMovimientoStockResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var tipos = await _unitOfWork.TiposMovimientoStock.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<TipoMovimientoStockResponseDto>>.Ok(_mapper.Map<IEnumerable<TipoMovimientoStockResponseDto>>(tipos));
    }

    public async Task<BaseResponseDto<TipoMovimientoStockResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposMovimientoStock.GetByIdAsync(id, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoMovimientoStock), id);

        return BaseResponseDto<TipoMovimientoStockResponseDto>.Ok(_mapper.Map<TipoMovimientoStockResponseDto>(tipo));
    }

    public async Task<BaseResponseDto<TipoMovimientoStockResponseDto>> CrearAsync(CrearTipoMovimientoStockRequestDto request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.TiposMovimientoStock.ExisteCodigoAsync(request.Codigo, cancellationToken))
            throw new BusinessException($"Ya existe un tipo de movimiento de stock con el código '{request.Codigo}'.");

        if (request.Signo != -1 && request.Signo != 1)
            throw new BusinessException("El signo debe ser -1 o 1.");

        var userId = _currentUserService.UserId ?? 1;

        var tipo = new TipoMovimientoStock
        {
            Codigo = request.Codigo,
            Descripcion = request.Descripcion,
            Signo = request.Signo,
            Activo = true,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.TiposMovimientoStock.AddAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<TipoMovimientoStockResponseDto>.Ok(_mapper.Map<TipoMovimientoStockResponseDto>(tipo), "Tipo de movimiento de stock creado correctamente.");
    }

    public async Task<BaseResponseDto<TipoMovimientoStockResponseDto>> ActualizarAsync(ActualizarTipoMovimientoStockRequestDto request, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposMovimientoStock.GetByIdAsync(request.IdTipoMovimientoStock, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoMovimientoStock), request.IdTipoMovimientoStock);

        var existente = await _unitOfWork.TiposMovimientoStock.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existente is not null && existente.IdTipoMovimientoStock != request.IdTipoMovimientoStock)
            throw new BusinessException($"Ya existe un tipo de movimiento de stock con el código '{request.Codigo}'.");

        if (request.Signo != -1 && request.Signo != 1)
            throw new BusinessException("El signo debe ser -1 o 1.");

        var userId = _currentUserService.UserId ?? 1;

        tipo.Codigo = request.Codigo;
        tipo.Descripcion = request.Descripcion;
        tipo.Signo = request.Signo;
        tipo.Activo = request.Activo;
        tipo.IdUsuarioUltimaModificacion = userId;
        tipo.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.TiposMovimientoStock.UpdateAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<TipoMovimientoStockResponseDto>.Ok(_mapper.Map<TipoMovimientoStockResponseDto>(tipo), "Tipo de movimiento de stock actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var tipo = await _unitOfWork.TiposMovimientoStock.GetByIdAsync(id, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoMovimientoStock), id);

        if (!tipo.Activo)
            throw new BusinessException("El tipo de movimiento de stock ya se encuentra dado de baja.");

        var userId = _currentUserService.UserId ?? 1;

        tipo.Activo = false;
        tipo.IdUsuarioUltimaModificacion = userId;
        tipo.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.TiposMovimientoStock.UpdateAsync(tipo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Tipo de movimiento de stock dado de baja correctamente.");
    }
}
