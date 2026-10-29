using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Stock;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IStockService
{
    Task<BaseResponseDto<IEnumerable<StockResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<StockResponseDto>> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<StockResponseDto>> CrearAsync(CrearStockRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<StockResponseDto>> ActualizarAsync(ActualizarStockRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(long id, CancellationToken cancellationToken = default);
}
