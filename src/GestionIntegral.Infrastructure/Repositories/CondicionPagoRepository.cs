using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class CondicionPagoRepository : Repository<CondicionPago>, ICondicionPagoRepository
{
    public CondicionPagoRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<CondicionPago?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCondicionPago == id, cancellationToken);
    }

    public async Task<CondicionPago?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Codigo == codigo, cancellationToken);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(c => c.Codigo == codigo, cancellationToken);
    }
}
