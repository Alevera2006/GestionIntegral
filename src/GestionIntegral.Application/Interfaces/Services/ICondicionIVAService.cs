using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.CondicionesIVA;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ICondicionIVAService
{
    Task<BaseResponseDto<IEnumerable<CondicionIVAResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CondicionIVAResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CondicionIVAResponseDto>> CrearAsync(CrearCondicionIVARequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CondicionIVAResponseDto>> ActualizarAsync(ActualizarCondicionIVARequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
