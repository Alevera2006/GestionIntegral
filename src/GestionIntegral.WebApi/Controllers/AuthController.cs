using GestionIntegral.Application.DTOs.Auth;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(request, cancellationToken);
        return Result(response);
    }
}
