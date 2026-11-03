using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Facturacion;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IFacturaService
{
    Task<BaseResponseDto<IEnumerable<FacturaResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<BaseResponseDto<FacturaResponseDto>> CrearAsync(FacturaRequestDto request, CancellationToken cancellationToken = default);
}
