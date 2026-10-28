using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class SubCategoriaRepository : Repository<SubCategoria>, ISubCategoriaRepository
{
    public SubCategoriaRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<SubCategoria?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(s => s.Categoria)
            .FirstOrDefaultAsync(s => s.IdSubCategoria == id, cancellationToken);
    }

    public override async Task<IEnumerable<SubCategoria>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(s => s.Categoria)
            .ToListAsync(cancellationToken);
    }

    public async Task<SubCategoria?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(s => s.Categoria)
            .FirstOrDefaultAsync(s => s.Codigo == codigo, cancellationToken);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(s => s.Codigo == codigo, cancellationToken);
    }

    public async Task<IEnumerable<SubCategoria>> GetByCategoriaAsync(int idCategoria, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(s => s.Categoria)
            .Where(s => s.IdCategoria == idCategoria)
            .ToListAsync(cancellationToken);
    }
}
