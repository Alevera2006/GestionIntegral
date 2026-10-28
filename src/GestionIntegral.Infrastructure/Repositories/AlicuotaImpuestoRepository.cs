using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class AlicuotaImpuestoRepository : Repository<AlicuotaImpuesto>, IAlicuotaImpuestoRepository
{
    public AlicuotaImpuestoRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<AlicuotaImpuesto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(a => a.TipoImpuesto)
            .FirstOrDefaultAsync(a => a.IdAlicuotaImpuesto == id, cancellationToken);
    }

    public override async Task<IEnumerable<AlicuotaImpuesto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(a => a.TipoImpuesto)
            .ToListAsync(cancellationToken);
    }

    public async Task<AlicuotaImpuesto?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(a => a.TipoImpuesto)
            .FirstOrDefaultAsync(a => a.Codigo == codigo, cancellationToken);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(a => a.Codigo == codigo, cancellationToken);
    }

    public async Task<IEnumerable<AlicuotaImpuesto>> GetByTipoImpuestoAsync(int idTipoImpuesto, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(a => a.TipoImpuesto)
            .Where(a => a.IdTipoImpuesto == idTipoImpuesto)
            .ToListAsync(cancellationToken);
    }
}
