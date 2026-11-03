using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IPagoClienteRepository : IRepository<PagoCliente>
{
    Task<IEnumerable<PagoCliente>> GetByClienteAsync(int idCliente, CancellationToken cancellationToken = default);
}
