using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.MotivosAnulacion;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IMotivoAnulacionService
{
    Task<BaseResponseDto<IEnumerable<MotivoAnulacionResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MotivoAnulacionResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MotivoAnulacionResponseDto>> CrearAsync(CrearMotivoAnulacionRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<MotivoAnulacionResponseDto>> ActualizarAsync(ActualizarMotivoAnulacionRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
