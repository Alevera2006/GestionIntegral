using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GestionIntegral.WebClient.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiClient> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetAuthorizationToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void RemoveAuthorizationToken()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<TResponse?> GetAsync<TResponse>(string uri, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await DeserializeResponse<TResponse>(response, cancellationToken);
        }
        catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation(ex, "GET {Uri} canceled by request token.", uri);
            throw;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "GET {Uri} canceled due to timeout or transport interruption.", uri);
            throw;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest request, CancellationToken cancellationToken = default)
    {
        var content = SerializeRequest(request);
        var response = await _httpClient.PostAsync(uri, content, cancellationToken);
        return await DeserializeResponse<TResponse>(response, cancellationToken);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string uri, TRequest request, CancellationToken cancellationToken = default)
    {
        var content = SerializeRequest(request);
        var response = await _httpClient.PutAsync(uri, content, cancellationToken);
        return await DeserializeResponse<TResponse>(response, cancellationToken);
    }

    public async Task<bool> DeleteAsync(string uri, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(uri, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogWarning("Token expirado o no autorizado. Redirigiendo al login.");
            RedirigirALogin();
            return false;
        }
        return response.IsSuccessStatusCode;
    }

    private static StringContent SerializeRequest<TRequest>(TRequest request)
    {
        var json = JsonSerializer.Serialize(request, JsonOptions);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private async Task<TResponse?> DeserializeResponse<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogWarning("Token expirado o no autorizado. Redirigiendo al login.");
            RedirigirALogin();
            return default;
        }

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(content))
            return default;

        return JsonSerializer.Deserialize<TResponse>(content, JsonOptions);
    }

    private void RedirigirALogin()
    {
        try
        {
            var context = _httpContextAccessor.HttpContext;
            if (context is null) return;

            var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;
            if (path.StartsWith("/account/login")) return;

            context.Response.Redirect("/Account/Login?ReturnUrl=" + Uri.EscapeDataString(context.Request.Path + context.Request.QueryString));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al redirigir al login por token expirado.");
        }
    }
}
