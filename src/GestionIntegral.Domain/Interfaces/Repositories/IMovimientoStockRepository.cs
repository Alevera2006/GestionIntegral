using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IMovimientoStockRepository : IRepository<MovimientoStock>
{
    Task<MovimientoStock?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}
