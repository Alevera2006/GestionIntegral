using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class StockRepository : Repository<Stock>, IStockRepository
{
    public StockRepository(GestionIntegralDbContext context) : base(context) { }

    public async Task<Stock?> GetByDepositoProductoAsync(int idDeposito, int idProducto, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.IdDeposito == idDeposito && s.IdProducto == idProducto, cancellationToken);
    }
}
