using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.TiposUsuario;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ITipoUsuarioService
{
    Task<BaseResponseDto<IEnumerable<TipoUsuarioResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoUsuarioResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<TipoUsuarioResponseDto>> CrearAsync(CrearTipoUsuarioRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
