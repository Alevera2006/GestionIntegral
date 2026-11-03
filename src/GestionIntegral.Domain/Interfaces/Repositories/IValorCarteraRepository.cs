using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IValorCarteraRepository : IRepository<ValorCartera>
{
    Task<IEnumerable<ValorCartera>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
}
