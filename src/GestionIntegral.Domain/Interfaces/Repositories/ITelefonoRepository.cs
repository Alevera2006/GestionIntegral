using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface ITelefonoRepository : IRepository<Telefono>
{
    Task<IEnumerable<Telefono>> GetByOwnerAsync(string? tipoReferencia, int? idReferencia, CancellationToken cancellationToken = default);
}
