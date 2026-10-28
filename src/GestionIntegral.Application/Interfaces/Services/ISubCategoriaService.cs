using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.SubCategorias;

namespace GestionIntegral.Application.Interfaces.Services;

public interface ISubCategoriaService
{
    Task<BaseResponseDto<IEnumerable<SubCategoriaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<IEnumerable<SubCategoriaResponseDto>>> ObtenerPorCategoriaAsync(int idCategoria, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<SubCategoriaResponseDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<SubCategoriaResponseDto>> CrearAsync(CrearSubCategoriaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<SubCategoriaResponseDto>> ActualizarAsync(ActualizarSubCategoriaRequestDto request, CancellationToken cancellationToken = default);
    Task<BaseResponseDto<bool>> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
