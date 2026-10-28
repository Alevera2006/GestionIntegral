using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IAlicuotaImpuestoRepository : IRepository<AlicuotaImpuesto>
{
    Task<AlicuotaImpuesto?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<IEnumerable<AlicuotaImpuesto>> GetByTipoImpuestoAsync(int idTipoImpuesto, CancellationToken cancellationToken = default);
}
