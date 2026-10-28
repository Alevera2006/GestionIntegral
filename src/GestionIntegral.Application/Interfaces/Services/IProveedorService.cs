using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Proveedores;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IProveedorService
{
    Task<BaseResponseDto<IEnumerable<ProveedorResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ProveedorResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ProveedorResponseDto>> CrearAsync(CrearProveedorRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<ProveedorResponseDto>> ActualizarAsync(ActualizarProveedorRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
