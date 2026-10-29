using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GestionIntegral.WebClient.Controllers;

[Authorize]
public abstract class BaseController : Controller
{
    protected readonly IApiClient ApiClient;

    protected BaseController(IApiClient apiClient)
    {
        ApiClient = apiClient;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var token = User.FindFirst("AccessToken")?.Value;

        if (!string.IsNullOrEmpty(token))
            ApiClient.SetAuthorizationToken(token);
        else
            ApiClient.RemoveAuthorizationToken();

        base.OnActionExecuting(context);
    }
}
