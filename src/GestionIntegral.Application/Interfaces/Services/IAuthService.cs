using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.Auth;

namespace GestionIntegral.Application.Interfaces.Services;

public interface IAuthService
{
    Task<BaseResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
}
