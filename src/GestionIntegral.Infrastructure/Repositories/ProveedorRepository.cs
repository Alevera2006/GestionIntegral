using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class ProveedorRepository : Repository<Proveedor>, IProveedorRepository
{
    public ProveedorRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<Proveedor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.CondicionPago)
            .FirstOrDefaultAsync(p => p.IdProveedor == id, cancellationToken);
    }

    public override async Task<IEnumerable<Proveedor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.CondicionPago)
            .ToListAsync(cancellationToken);
    }

    public async Task<Proveedor?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.CondicionPago)
            .FirstOrDefaultAsync(p => p.Codigo == codigo, cancellationToken);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(p => p.Codigo == codigo, cancellationToken);
    }
}
