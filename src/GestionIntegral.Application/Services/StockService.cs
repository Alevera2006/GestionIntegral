using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Stock;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;

namespace GestionIntegral.Application.Services;

public class StockService : IStockService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public StockService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseDto<IEnumerable<StockResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var stocks = await _unitOfWork.Stock.GetAllAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<StockResponseDto>>.Ok(_mapper.Map<IEnumerable<StockResponseDto>>(stocks));
    }

    public async Task<BaseResponseDto<StockResponseDto>> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var stock = await _unitOfWork.Stock.GetByIdAsync((int)id, cancellationToken);
        if (stock is null)
            throw new NotFoundException(nameof(Stock), id);

        return BaseResponseDto<StockResponseDto>.Ok(_mapper.Map<StockResponseDto>(stock));
    }

    public async Task<BaseResponseDto<StockResponseDto>> CrearAsync(CrearStockRequestDto request, CancellationToken cancellationToken = default)
    {
        var deposito = await _unitOfWork.Depositos.GetByIdAsync(request.IdDeposito, cancellationToken);
        if (deposito is null)
            throw new NotFoundException(nameof(Deposito), request.IdDeposito);

        var producto = await _unitOfWork.Productos.GetByIdAsync(request.IdProducto, cancellationToken);
        if (producto is null)
            throw new NotFoundException(nameof(Producto), request.IdProducto);

        var existente = await _unitOfWork.Stock.GetByDepositoProductoAsync(request.IdDeposito, request.IdProducto, cancellationToken);
        if (existente is not null)
            throw new BusinessException("Ya existe un registro de stock para el depósito y producto indicados.");

        var stock = new Stock
        {
            IdDeposito = request.IdDeposito,
            IdProducto = request.IdProducto,
            Cantidad = request.Cantidad
        };

        await _unitOfWork.Stock.AddAsync(stock, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<StockResponseDto>.Ok(_mapper.Map<StockResponseDto>(stock), "Stock creado correctamente.");
    }

    public async Task<BaseResponseDto<StockResponseDto>> ActualizarAsync(ActualizarStockRequestDto request, CancellationToken cancellationToken = default)
    {
        var stock = await _unitOfWork.Stock.GetByIdAsync((int)request.IdStock, cancellationToken);
        if (stock is null)
            throw new NotFoundException(nameof(Stock), request.IdStock);

        stock.Cantidad = request.Cantidad;
        stock.FechaUltimoMovimiento = DateTime.UtcNow;

        await _unitOfWork.Stock.UpdateAsync(stock, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<StockResponseDto>.Ok(_mapper.Map<StockResponseDto>(stock), "Stock actualizado correctamente.");
    }

    public async Task<BaseResponseDto<bool>> EliminarAsync(long id, CancellationToken cancellationToken = default)
    {
        var stock = await _unitOfWork.Stock.GetByIdAsync((int)id, cancellationToken);
        if (stock is null)
            throw new NotFoundException(nameof(Stock), id);

        await _unitOfWork.Stock.DeleteAsync(stock, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponseDto<bool>.Ok(true, "Stock eliminado correctamente.");
    }
}
