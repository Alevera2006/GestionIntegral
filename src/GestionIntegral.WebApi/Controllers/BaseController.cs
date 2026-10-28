using GestionIntegral.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult Result<T>(BaseResponseDto<T> response)
    {
        if (!response.Exitoso)
            return BadRequest(response);

        return Ok(response);
    }
}
