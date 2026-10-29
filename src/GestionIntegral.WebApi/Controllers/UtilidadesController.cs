using GestionIntegral.Application.DTOs;
using GestionIntegral.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

/// <summary>
/// Controlador de utilidades para desarrollo.
/// Solo disponible en entornos de desarrollo.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UtilidadesController : ControllerBase
{
    private readonly IPasswordService _passwordService;
    private readonly IWebHostEnvironment _environment;

    public UtilidadesController(IPasswordService passwordService, IWebHostEnvironment environment)
    {
        _passwordService = passwordService;
        _environment = environment;
    }

    /// <summary>
    /// Genera un hash BCrypt a partir de una clave en texto plano.
    /// Endpoint disponible unicamente en entorno de desarrollo.
    /// </summary>
    /// <param name="request">Clave a hashear.</param>
    /// <returns>Hash BCrypt de 60 caracteres.</returns>
    [HttpPost("generar-hash")]
    public IActionResult GenerarHash([FromBody] GenerarHashRequestDto request)
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        if (string.IsNullOrWhiteSpace(request.Clave))
            return BadRequest(BaseResponseDto<string>.Error("La clave es requerida."));

        var hash = _passwordService.Hash(request.Clave);

        return Ok(BaseResponseDto<string>.Ok(hash, "Hash generado correctamente."));
    }
}

public class GenerarHashRequestDto
{
    public string Clave { get; set; } = string.Empty;
}
