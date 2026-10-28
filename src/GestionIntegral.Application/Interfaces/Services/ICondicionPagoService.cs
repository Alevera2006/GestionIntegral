using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.CondicionesPago;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ICondicionPagoService
{
    Task<BaseResponseDto<IEnumerable<CondicionPagoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CondicionPagoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CondicionPagoResponseDto>> CrearAsync(CrearCondicionPagoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CondicionPagoResponseDto>> ActualizarAsync(ActualizarCondicionPagoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
