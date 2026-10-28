using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface ICondicionPagoRepository : IRepository<CondicionPago>
{
    Task<CondicionPago?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default);
}
