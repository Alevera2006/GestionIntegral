using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.MovimientosStock;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class MovimientoStockDetalleService : IMovimientoStockDetalleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public MovimientoStockDetalleService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<MovimientoStockDetalleResponseDto>>> ObtenerPorMovimientoAsync(long idMovimientoStock, CancellationToken cancellationToken = default)
    {
        var detalles = await _unitOfWork.MovimientoStockDetalle.FindAsync(d => d.IdMovimientoStock == idMovimientoStock, cancellationToken);
        return BaseResponseDto<IEnumerable<MovimientoStockDetalleResponseDto>>.Ok(_mapper.Map<IEnumerable<MovimientoStockDetalleResponseDto>>(detalles));
    }

    public async Task<BaseResponseDto<MovimientoStockDetalleResponseDto>> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var detalle = await _unitOfWork.MovimientoStockDetalle.GetByIdAsync((int)id, cancellationToken);
        if (detalle is null)
            throw new NotFoundException(nameof(MovimientoStockDetalle), id);

        return BaseResponseDto<MovimientoStockDetalleResponseDto>.Ok(_mapper.Map<MovimientoStockDetalleResponseDto>(detalle));
    }

    public async Task<BaseResponseDto<MovimientoStockDetalleResponseDto>> CrearAsync(CrearMovimientoStockDetalleRequestDto request, CancellationToken cancellationToken = default)
    {
        var movimiento = await _unitOfWork.MovimientosStock.GetByIdAsync((int)request.IdMovimientoStock, cancellationToken);
        if (movimiento is null)
            throw new NotFoundException(nameof(MovimientoStock), request.IdMovimientoStock);

        var producto = await _unitOfWork.Productos.GetByIdAsync(request.IdProducto, cancellationToken);
        if (producto is null)
            throw new NotFoundException(nameof(Producto), request.IdProducto);

        if (request.Cantidad <= 0)
            throw new BusinessException("La cantidad debe ser mayor a cero.");

        var userId = _currentUserService.UserId ?? 1;

        var detalle = new MovimientoStockDetalle
        {
            IdMovimientoStock = request.IdMovimientoStock,
            IdProducto = request.IdProducto,
            Cantidad = request.Cantidad,
            IdUsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.MovimientoStockDetalle.AddAsync(detalle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<MovimientoStockDetalleResponseDto>.Ok(_mapper.Map<MovimientoStockDetalleResponseDto>(detalle), "Detalle de movimiento de stock creado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(long id, CancellationToken cancellationToken = default)
    {
        var detalle = await _unitOfWork.MovimientoStockDetalle.GetByIdAsync((int)id, cancellationToken);
        if (detalle is null)
            throw new NotFoundException(nameof(MovimientoStockDetalle), id);

        await _unitOfWork.MovimientoStockDetalle.DeleteAsync(detalle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Detalle de movimiento de stock eliminado correctamente.");
    }
}
