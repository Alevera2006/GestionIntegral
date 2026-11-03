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

    public async Task<Stock?> GetByDepositoProductoTrackedAsync(int idDeposito, int idProducto, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.IdDeposito == idDeposito && s.IdProducto == idProducto, cancellationToken);
    }

    public async Task<IEnumerable<Stock>> GetByFiltroAsync(int? idDeposito, int? idCategoria, int? idSubCategoria, string? busquedaProducto, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .AsNoTracking()
            .Include(s => s.Deposito)
            .Include(s => s.Producto)
            .AsQueryable();

        if (idDeposito.HasValue)
            query = query.Where(s => s.IdDeposito == idDeposito.Value);

        if (idCategoria.HasValue)
            query = query.Where(s => s.Producto.IdCategoria == idCategoria.Value);

        if (idSubCategoria.HasValue)
            query = query.Where(s => s.Producto.IdSubCategoria == idSubCategoria.Value);

        if (!string.IsNullOrWhiteSpace(busquedaProducto))
        {
            var busqueda = busquedaProducto.Trim().ToLower();
            query = query.Where(s =>
                (s.Producto.Codigo != null && s.Producto.Codigo.ToLower().Contains(busqueda)) ||
                (s.Producto.Descripcion != null && s.Producto.Descripcion.ToLower().Contains(busqueda)));
        }

        return await query.ToListAsync(cancellationToken);
    }
}
