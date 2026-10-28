using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class ListaPrecioRepository : Repository<ListaPrecio>, IListaPrecioRepository
{
    public ListaPrecioRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<ListaPrecio?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.IdListaPrecio == id, cancellationToken);
    }

    public async Task<ListaPrecio?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Codigo == codigo, cancellationToken);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(l => l.Codigo == codigo, cancellationToken);
    }

    public async Task<ListaPrecio?> GetByIdWithDetallesAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(l => l.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(l => l.IdListaPrecio == id, cancellationToken);
    }
}
