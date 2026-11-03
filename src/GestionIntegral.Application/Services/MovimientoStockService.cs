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
        var movimiento = await _unitOfWork.MovimientosStock.GetByIdAsync(id, cancellationToken);
        if (movimiento is null)
            throw new NotFoundException(nameof(MovimientoStock), id);

        return BaseResponseDto<MovimientoStockResponseDto>.Ok(_mapper.Map<MovimientoStockResponseDto>(movimiento));
    }

    public async Task<BaseResponseDto<MovimientoStockResponseDto>> CrearAsync(CrearMovimientoStockRequestDto request, CancellationToken cancellationToken = default)
    {
        var movimiento = await CrearEntidadAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<MovimientoStockResponseDto>.Ok(_mapper.Map<MovimientoStockResponseDto>(movimiento), "Movimiento de stock creado correctamente.");
    }

    public async Task<BaseResponseDto<MovimientoStockResponseDto>> CrearConDetallesAsync(CrearMovimientoStockConDetallesRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.Detalles is null || request.Detalles.Count == 0)
            throw new BusinessException("El movimiento debe tener al menos un detalle.");

        var baseRequest = new CrearMovimientoStockRequestDto
        {
            IdDeposito = request.IdDeposito,
            IdTipoMovimientoStock = request.IdTipoMovimientoStock,
            IdUsuarioResponsable = request.IdUsuarioResponsable,
            Referencia = request.Referencia,
            Observaciones = request.Observaciones
        };

        var movimiento = await CrearEntidadAsync(baseRequest, cancellationToken);
        var tipoMovimiento = await _unitOfWork.TiposMovimientoStock.GetByIdAsync(request.IdTipoMovimientoStock, cancellationToken);
        if (tipoMovimiento is null)
            throw new NotFoundException(nameof(TipoMovimientoStock), request.IdTipoMovimientoStock);

        var userId = _currentUserService.UserId ?? 1;

        foreach (var item in request.Detalles)
        {
            if (item.Cantidad <= 0)
                throw new BusinessException("La cantidad de cada detalle debe ser mayor a cero.");

            var producto = await _unitOfWork.Productos.GetByIdAsync(item.IdProducto, cancellationToken);
            if (producto is null)
                throw new NotFoundException(nameof(Producto), item.IdProducto);

            var detalle = new MovimientoStockDetalle
            {
                IdMovimientoStock = movimiento.IdMovimientoStock,
                IdProducto = item.IdProducto,
                Cantidad = item.Cantidad,
                IdUsuarioCreacion = userId,
                FechaCreacion = DateTime.UtcNow
            };

            await _unitOfWork.MovimientoStockDetalle.AddAsync(detalle, cancellationToken);
            await AjustarStockAsync(request.IdDeposito, item.IdProducto, item.Cantidad * tipoMovimiento.Signo, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<MovimientoStockResponseDto>.Ok(_mapper.Map<MovimientoStockResponseDto>(movimiento), "Movimiento de stock creado correctamente.");
    }

    private async Task<MovimientoStock> CrearEntidadAsync(CrearMovimientoStockRequestDto request, CancellationToken cancellationToken)
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
        return movimiento;
    }

    private async Task AjustarStockAsync(int idDeposito, int idProducto, decimal cantidadAjuste, CancellationToken cancellationToken)
    {
        var stock = await _unitOfWork.Stock.GetByDepositoProductoTrackedAsync(idDeposito, idProducto, cancellationToken);

        if (stock is null)
        {
            if (cantidadAjuste < 0)
                throw new BusinessException("No hay stock disponible para realizar la baja.");

            stock = new Stock
            {
                IdDeposito = idDeposito,
                IdProducto = idProducto,
                Cantidad = cantidadAjuste,
                FechaUltimoMovimiento = DateTime.UtcNow
            };

            await _unitOfWork.Stock.AddAsync(stock, cancellationToken);
        }
        else
        {
            var nuevaCantidad = stock.Cantidad + cantidadAjuste;
            if (nuevaCantidad < 0)
                throw new BusinessException("La operación dejaría el stock negativo.");

            stock.Cantidad = nuevaCantidad;
            stock.FechaUltimoMovimiento = DateTime.UtcNow;

            await _unitOfWork.Stock.UpdateAsync(stock, cancellationToken);
        }
    }

    public async Task<BaseResponseDto<MovimientoStockResponseDto>> ActualizarAsync(ActualizarMovimientoStockRequestDto request, CancellationToken cancellationToken = default)
    {
        var movimiento = await _unitOfWork.MovimientosStock.GetByIdAsync(request.IdMovimientoStock, cancellationToken);
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
        var movimiento = await _unitOfWork.MovimientosStock.GetByIdAsync(id, cancellationToken);
        if (movimiento is null)
            throw new NotFoundException(nameof(MovimientoStock), id);

        var tipoMovimiento = await _unitOfWork.TiposMovimientoStock.GetByIdAsync(movimiento.IdTipoMovimientoStock, cancellationToken);
        if (tipoMovimiento is null)
            throw new NotFoundException(nameof(TipoMovimientoStock), movimiento.IdTipoMovimientoStock);

        var detalles = await _unitOfWork.MovimientoStockDetalle.FindAsync(d => d.IdMovimientoStock == id, cancellationToken);

        foreach (var detalle in detalles)
        {
            await AjustarStockAsync(
                movimiento.IdDeposito,
                detalle.IdProducto,
                -detalle.Cantidad * tipoMovimiento.Signo,
                cancellationToken);
        }

        await _unitOfWork.MovimientosStock.DeleteAsync(movimiento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Movimiento de stock eliminado correctamente.");
    }

    public async Task<BaseResponseDto<IEnumerable<HistorialStockPorProductoResponseDto>>> ObtenerHistorialPorProductoAsync(int idProducto, int? idDeposito, CancellationToken cancellationToken = default)
    {
        var producto = await _unitOfWork.Productos.GetByIdAsync(idProducto, cancellationToken);
        if (producto is null)
            throw new NotFoundException(nameof(Producto), idProducto);

        if (idDeposito.HasValue)
        {
            var deposito = await _unitOfWork.Depositos.GetByIdAsync(idDeposito.Value, cancellationToken);
            if (deposito is null)
                throw new NotFoundException(nameof(Deposito), idDeposito.Value);
        }

        var detalles = await _unitOfWork.MovimientoStockDetalle.GetByProductoAsync(idProducto, idDeposito, cancellationToken);
        return BaseResponseDto<IEnumerable<HistorialStockPorProductoResponseDto>>.Ok(_mapper.Map<IEnumerable<HistorialStockPorProductoResponseDto>>(detalles));
    }
}
