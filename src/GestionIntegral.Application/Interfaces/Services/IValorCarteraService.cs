using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.ValoresCartera;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IValorCarteraService
{
    Task<BaseResponseDto<IEnumerable<ValorCarteraResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
}
