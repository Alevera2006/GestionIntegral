using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class ClienteRepository : Repository<Cliente>, IClienteRepository
{
    public ClienteRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<Cliente?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.CondicionPago)
            .FirstOrDefaultAsync(c => c.IdCliente == id, cancellationToken);
    }

    public override async Task<IEnumerable<Cliente>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.CondicionPago)
            .ToListAsync(cancellationToken);
    }

    public async Task<Cliente?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.CondicionPago)
            .FirstOrDefaultAsync(c => c.Codigo == codigo, cancellationToken);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(c => c.Codigo == codigo, cancellationToken);
    }
}
