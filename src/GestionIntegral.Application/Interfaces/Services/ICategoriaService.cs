using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Categorias;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ICategoriaService
{
    Task<BaseResponseDto<IEnumerable<CategoriaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CategoriaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CategoriaResponseDto>> CrearAsync(CrearCategoriaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<CategoriaResponseDto>> ActualizarAsync(ActualizarCategoriaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
