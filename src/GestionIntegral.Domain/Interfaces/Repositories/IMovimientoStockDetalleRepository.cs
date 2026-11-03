using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IMovimientoStockDetalleRepository : IRepository<MovimientoStockDetalle>
{
    Task<MovimientoStockDetalle?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MovimientoStockDetalle>> GetByProductoAsync(int idProducto, int? idDeposito, CancellationToken cancellationToken = default);
}
