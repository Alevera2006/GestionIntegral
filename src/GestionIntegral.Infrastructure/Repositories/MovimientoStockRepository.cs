using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class MovimientoStockRepository : Repository<MovimientoStock>, IMovimientoStockRepository
{
    public MovimientoStockRepository(GestionIntegralDbContext context) : base(context) { }

    public async Task<MovimientoStock?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }
}
