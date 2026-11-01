using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IEmailRepository : IRepository<Email>
{
    Task<IEnumerable<Email>> GetByOwnerAsync(string? tipoReferencia, int? idReferencia, CancellationToken cancellationToken = default);
}
