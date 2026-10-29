using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.TiposMovimientoStock;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ITipoMovimientoStockService
{
    Task<BaseResponseDto<IEnumerable<TipoMovimientoStockResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoMovimientoStockResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoMovimientoStockResponseDto>> CrearAsync(CrearTipoMovimientoStockRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoMovimientoStockResponseDto>> ActualizarAsync(ActualizarTipoMovimientoStockRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
