namespace GestionIntegral.WebClient.Services;

public interface IApiClient
{
    Task<TResponse?> GetAsync<TResponse>(string uri, CancellationToken cancellationToken = default);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest request, CancellationToken cancellationToken = default);
    Task<TResponse?> PutAsync<TRequest, TResponse>(string uri, TRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string uri, CancellationToken cancellationToken = default);
    void SetAuthorizationToken(string token);
    void RemoveAuthorizationToken();
}
