using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IStockRepository : IRepository<Stock>
{
    Task<Stock?> GetByDepositoProductoAsync(int idDeposito, int idProducto, CancellationToken cancellationToken = default);
}
