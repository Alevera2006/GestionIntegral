using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.MovimientosStock;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IMovimientoStockService
{
    Task<BaseResponseDto<IEnumerable<MovimientoStockResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MovimientoStockResponseDto>> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MovimientoStockResponseDto>> CrearAsync(CrearMovimientoStockRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MovimientoStockResponseDto>> ActualizarAsync(ActualizarMovimientoStockRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(long id, CancellationToken cancellationToken = default);
}
