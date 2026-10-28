using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface ISubCategoriaRepository : IRepository<SubCategoria>
{
    Task<SubCategoria?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<IEnumerable<SubCategoria>> GetByCategoriaAsync(int idCategoria, CancellationToken cancellationToken = default);
}
