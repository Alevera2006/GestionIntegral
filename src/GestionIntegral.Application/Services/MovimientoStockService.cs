using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.MovimientosStock;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class MovimientoStockService : IMovimientoStockService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public MovimientoStockService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<MovimientoStockResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var movimientos = await _unitOfWork.MovimientosStock.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<MovimientoStockResponseDto>>.Ok(_mapper.Map<IEnumerable<MovimientoStockResponseDto>>(movimientos));
    }

    public async Task<BaseResponseDto<MovimientoStockResponseDto>> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var movimiento = await _unitOfWork.MovimientosStock.GetByIdAsync((int)id, cancellationToken);
        if (movimiento is null)
            throw new NotFoundException(nameof(MovimientoStock), id);

        return BaseResponseDto<MovimientoStockResponseDto>.Ok(_mapper.Map<MovimientoStockResponseDto>(movimiento));
    }

    public async Task<BaseResponseDto<MovimientoStockResponseDto>> CrearAsync(CrearMovimientoStockRequestDto request, CancellationToken cancellationToken = default)
    {
        var deposito = await _unitOfWork.Depositos.GetByIdAsync(request.IdDeposito, cancellationToken);
        if (deposito is null)
            throw new NotFoundException(nameof(Deposito), request.IdDeposito);

        var tipo = await _unitOfWork.TiposMovimientoStock.GetByIdAsync(request.IdTipoMovimientoStock, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoMovimientoStock), request.IdTipoMovimientoStock);

        var responsable = await _unitOfWork.Usuarios.GetByIdAsync(request.IdUsuarioResponsable, cancellationToken);
        if (responsable is null)
            throw new NotFoundException(nameof(Usuario), request.IdUsuarioResponsable);

        var userId = _currentUserService.UserId ?? 1;

        var movimiento = new MovimientoStock
        {
            IdDeposito = request.IdDeposito,
            IdTipoMovimientoStock = request.IdTipoMovimientoStock,
            FechaMovimiento = DateTime.UtcNow,
            IdUsuarioResponsable = request.IdUsuarioResponsable,
            Referencia = request.Referencia,
            Observaciones = request.Observaciones,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.MovimientosStock.AddAsync(movimiento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<MovimientoStockResponseDto>.Ok(_mapper.Map<MovimientoStockResponseDto>(movimiento), "Movimiento de stock creado correctamente.");
    }

    public async Task<BaseResponseDto<MovimientoStockResponseDto>> ActualizarAsync(ActualizarMovimientoStockRequestDto request, CancellationToken cancellationToken = default)
    {
        var movimiento = await _unitOfWork.MovimientosStock.GetByIdAsync((int)request.IdMovimientoStock, cancellationToken);
        if (movimiento is null)
            throw new NotFoundException(nameof(MovimientoStock), request.IdMovimientoStock);

        var deposito = await _unitOfWork.Depositos.GetByIdAsync(request.IdDeposito, cancellationToken);
        if (deposito is null)
            throw new NotFoundException(nameof(Deposito), request.IdDeposito);

        var tipo = await _unitOfWork.TiposMovimientoStock.GetByIdAsync(request.IdTipoMovimientoStock, cancellationToken);
        if (tipo is null)
            throw new NotFoundException(nameof(TipoMovimientoStock), request.IdTipoMovimientoStock);

        var responsable = await _unitOfWork.Usuarios.GetByIdAsync(request.IdUsuarioResponsable, cancellationToken);
        if (responsable is null)
            throw new NotFoundException(nameof(Usuario), request.IdUsuarioResponsable);

        var userId = _currentUserService.UserId ?? 1;

        movimiento.IdDeposito = request.IdDeposito;
        movimiento.IdTipoMovimientoStock = request.IdTipoMovimientoStock;
        movimiento.IdUsuarioResponsable = request.IdUsuarioResponsable;
        movimiento.Referencia = request.Referencia;
        movimiento.Observaciones = request.Observaciones;
        movimiento.IdUsuarioUltimaModificacion = userId;
        movimiento.FechaUltimaModificacion = DateTime.UtcNow;

        await _unitOfWork.MovimientosStock.UpdateAsync(movimiento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<MovimientoStockResponseDto>.Ok(_mapper.Map<MovimientoStockResponseDto>(movimiento), "Movimiento de stock actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(long id, CancellationToken cancellationToken = default)
    {
        var movimiento = await _unitOfWork.MovimientosStock.GetByIdAsync((int)id, cancellationToken);
        if (movimiento is null)
            throw new NotFoundException(nameof(MovimientoStock), id);

        await _unitOfWork.MovimientosStock.DeleteAsync(movimiento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Movimiento de stock eliminado correctamente.");
    }
}
