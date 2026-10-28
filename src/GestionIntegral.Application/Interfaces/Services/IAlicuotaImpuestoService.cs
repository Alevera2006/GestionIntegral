using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.AlicuotasImpuesto;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IAlicuotaImpuestoService
{
    Task<BaseResponseDto<IEnumerable<AlicuotaImpuestoResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<IEnumerable<AlicuotaImpuestoResponseDto>>> ObtenerPorTipoImpuestoAsync(int idTipoImpuesto, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<AlicuotaImpuestoResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<AlicuotaImpuestoResponseDto>> CrearAsync(CrearAlicuotaImpuestoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<AlicuotaImpuestoResponseDto>> ActualizarAsync(ActualizarAlicuotaImpuestoRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
