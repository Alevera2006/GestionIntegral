namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IConfiguracionRepository<T> : IRepository<T> where T : class
{
    Task<T?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default);
}
