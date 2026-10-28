using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Usuarios;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IUsuarioService
{
    Task<BaseResponseDto<IEnumerable<UsuarioResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<UsuarioResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<UsuarioResponseDto>> CrearAsync(CrearUsuarioRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<UsuarioResponseDto>> ActualizarAsync(ActualizarUsuarioRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
