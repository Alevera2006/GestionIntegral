using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IDomicilioRepository : IRepository<Domicilio>
{
    Task<IEnumerable<Domicilio>> GetByOwnerAsync(string? tipoReferencia, int? idReferencia, CancellationToken cancellationToken = default);
}
