using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IClienteContactoRepository : IRepository<ClienteContacto>
{
    Task<IEnumerable<ClienteContacto>> GetByClienteAsync(int idCliente, CancellationToken cancellationToken = default);
    Task<ClienteContacto?> GetByIdsAsync(int idCliente, int idContacto, CancellationToken cancellationToken = default);
}
