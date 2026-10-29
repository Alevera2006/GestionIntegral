using GestionIntegral.Application.DTOs.ClienteContactos;
using GestionIntegral.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ClienteContactosController : BaseController
{
    private readonly IClienteContactoService _clienteContactoService;

    public ClienteContactosController(IClienteContactoService clienteContactoService)
    {
        _clienteContactoService = clienteContactoService;
    }

    [HttpGet("cliente/{idCliente:int}")]
    public async Task<IActionResult> GetByCliente(int idCliente, CancellationToken cancellationToken)
    {
        var response = await _clienteContactoService.ObtenerPorClienteAsync(idCliente, cancellationToken);
        return Result(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearClienteContactoRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _clienteContactoService.CrearAsync(request, cancellationToken);
        return Result(response);
    }

    [HttpDelete("cliente/{idCliente:int}/contacto/{idContacto:int}")]
    public async Task<IActionResult> Delete(int idCliente, int idContacto, CancellationToken cancellationToken)
    {
        var response = await _clienteContactoService.EliminarAsync(idCliente, idContacto, cancellationToken);
        return Result(response);
    }
}
