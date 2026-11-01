using GestionIntegral.Application.DTOs;
using Microsoft.AspNetCore.Http;
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

        if (HttpContext?.Request?.Method == HttpMethods.Post)
            return StatusCode(StatusCodes.Status201Created, response);

        return Ok(response);
    }
}
