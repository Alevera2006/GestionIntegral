using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class TipoImpuestoRepository : Repository<TipoImpuesto>, ITipoImpuestoRepository
{
    public TipoImpuestoRepository(GestionIntegralDbContext context) : base(context) { }

    public async Task<TipoImpuesto?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Codigo == codigo, cancellationToken);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(t => t.Codigo == codigo, cancellationToken);
    }
}
