using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class ListaPrecioDetalleRepository : Repository<ListaPrecioDetalle>, IListaPrecioDetalleRepository
{
    public ListaPrecioDetalleRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<IEnumerable<ListaPrecioDetalle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(d => d.Producto)
            .ToListAsync(cancellationToken);
    }

    public override async Task<ListaPrecioDetalle?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(d => d.Producto)
            .FirstOrDefaultAsync(d => d.IdListaPrecioDetalle == id, cancellationToken);
    }

    public async Task<bool> ExisteProductoEnListaAsync(int idListaPrecio, int idProducto, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(d => d.IdListaPrecio == idListaPrecio && d.IdProducto == idProducto, cancellationToken);
    }

    public async Task<IEnumerable<ListaPrecioDetalle>> GetByListaPrecioAsync(int idListaPrecio, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(d => d.Producto)
            .Where(d => d.IdListaPrecio == idListaPrecio)
            .ToListAsync(cancellationToken);
    }
}
