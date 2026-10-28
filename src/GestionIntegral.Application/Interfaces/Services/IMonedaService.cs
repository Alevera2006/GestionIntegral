using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Monedas;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IMonedaService
{
    Task<BaseResponseDto<IEnumerable<MonedaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MonedaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MonedaResponseDto>> CrearAsync(CrearMonedaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MonedaResponseDto>> ActualizarAsync(ActualizarMonedaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
