using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class ProductoRepository : Repository<Producto>, IProductoRepository
{
    public ProductoRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<Producto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.SubCategoria)
            .FirstOrDefaultAsync(p => p.IdProducto == id, cancellationToken);
    }

    public override async Task<IEnumerable<Producto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.SubCategoria)
            .ToListAsync(cancellationToken);
    }

    public async Task<Producto?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.SubCategoria)
            .FirstOrDefaultAsync(p => p.Codigo == codigo, cancellationToken);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(p => p.Codigo == codigo, cancellationToken);
    }
}
