using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GestionIntegral.WebClient.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
        var response = await _httpClient.GetAsync(uri, cancellationToken);
        return await DeserializeResponse<TResponse>(response, cancellationToken);
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
        return response.IsSuccessStatusCode;
    }

    private static StringContent SerializeRequest<TRequest>(TRequest request)
    {
        var json = JsonSerializer.Serialize(request, JsonOptions);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private static async Task<TResponse?> DeserializeResponse<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(content))
            return default;

        return JsonSerializer.Deserialize<TResponse>(content, JsonOptions);
    }
}
