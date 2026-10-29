using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.MovimientosStock;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IMovimientoStockDetalleService
{
    Task<BaseResponseDto<IEnumerable<MovimientoStockDetalleResponseDto>>> ObtenerPorMovimientoAsync(long idMovimientoStock, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MovimientoStockDetalleResponseDto>> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MovimientoStockDetalleResponseDto>> CrearAsync(CrearMovimientoStockDetalleRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(long id, CancellationToken cancellationToken = default);
}
