using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.MediosPago;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IMedioPagoService
{
    Task<BaseResponseDto<IEnumerable<MedioPagoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MedioPagoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MedioPagoResponseDto>> CrearAsync(CrearMedioPagoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MedioPagoResponseDto>> ActualizarAsync(ActualizarMedioPagoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
