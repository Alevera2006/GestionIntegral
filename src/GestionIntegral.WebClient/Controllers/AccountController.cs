using System.Security.Claims;
using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.Auth;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IApiClient _apiClient;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IApiClient apiClient, ILogger<AccountController> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToLocal(returnUrl);

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequestDto request, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            _apiClient.RemoveAuthorizationToken();
            var response = await _apiClient.PostAsync<LoginRequestDto, BaseResponseDto<LoginResponseDto>>(
                "api/auth/login",
                new LoginRequestDto { Usuario = request.Usuario, Clave = request.Clave });

            if (response is null || !response.Exitoso || response.Datos is null)
            {
                TempData["Error"] = response?.Mensaje ?? "Error al iniciar sesión.";
                return RedirectToAction(nameof(Login), new { returnUrl });
            }

            var datos = response.Datos;
            _apiClient.SetAuthorizationToken(datos.Token);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, datos.Usuario),
                new(ClaimTypes.Name, datos.NombreCompleto),
                new("Usuario", datos.Usuario),
                new("IdTipoUsuario", datos.IdTipoUsuario.ToString()),
                new("AccessToken", datos.Token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = request.Recordarme,
                ExpiresUtc = request.Recordarme
                    ? DateTimeOffset.UtcNow.AddDays(7)
                    : DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToLocal(returnUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en login");
            TempData["Error"] = "Ocurrió un error inesperado. Intente nuevamente.";
            return RedirectToAction(nameof(Login), new { returnUrl });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _apiClient.RemoveAuthorizationToken();
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }
}
