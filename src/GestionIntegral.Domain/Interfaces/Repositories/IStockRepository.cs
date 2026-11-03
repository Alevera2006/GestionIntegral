using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IStockRepository : IRepository<Stock>
{
    Task<Stock?> GetByDepositoProductoAsync(int idDeposito, int idProducto, CancellationToken cancellationToken = default);
    Task<Stock?> GetByDepositoProductoTrackedAsync(int idDeposito, int idProducto, CancellationToken cancellationToken = default);
    Task<IEnumerable<Stock>> GetByFiltroAsync(int? idDeposito, int? idCategoria, int? idSubCategoria, string? busquedaProducto, CancellationToken cancellationToken = default);
}
