using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class MovimientoStockDetalleRepository : Repository<MovimientoStockDetalle>, IMovimientoStockDetalleRepository
{
    public MovimientoStockDetalleRepository(GestionIntegralDbContext context) : base(context) { }

    public async Task<MovimientoStockDetalle?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<MovimientoStockDetalle>> GetByProductoAsync(int idProducto, int? idDeposito, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .AsNoTracking()
            .Include(d => d.MovimientoStock)
                .ThenInclude(m => m.Deposito)
            .Include(d => d.MovimientoStock)
                .ThenInclude(m => m.TipoMovimientoStock)
            .Include(d => d.MovimientoStock)
                .ThenInclude(m => m.UsuarioResponsable)
            .Include(d => d.Producto)
            .Where(d => d.IdProducto == idProducto)
            .AsQueryable();

        if (idDeposito.HasValue)
            query = query.Where(d => d.MovimientoStock.IdDeposito == idDeposito.Value);

        return await query
            .OrderByDescending(d => d.MovimientoStock.FechaMovimiento)
            .ThenByDescending(d => d.IdMovimientoStockDetalle)
            .ToListAsync(cancellationToken);
    }
}
