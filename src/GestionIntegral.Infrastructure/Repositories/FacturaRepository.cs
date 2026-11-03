using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class FacturaRepository : Repository<Factura>, IFacturaRepository
{
    public FacturaRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<IEnumerable<Factura>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await GetAllWithDetailsAsync(cancellationToken);
    }

    public async Task<IEnumerable<Factura>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(f => f.Cliente)
            .Include(f => f.TipoComprobante)
            .Include(f => f.CondicionPago)
            .Include(f => f.Detalles)
            .ThenInclude(d => d.Producto)
            .OrderByDescending(f => f.Fecha)
            .ToListAsync(cancellationToken);
    }

    public async Task<Factura?> GetByIdLongAsync(long id, CancellationToken cancellationToken = default)
    {
        return await GetByIdWithDetailsAsync(id, cancellationToken);
    }

    public async Task<Factura?> GetByIdWithDetailsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(f => f.Cliente)
            .Include(f => f.TipoComprobante)
            .Include(f => f.CondicionPago)
            .Include(f => f.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(f => f.IdFactura == id, cancellationToken);
    }

    public async Task<bool> ExisteNumeroComprobanteAsync(string numeroComprobante, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(f => f.NumeroComprobante == numeroComprobante, cancellationToken);
    }
}
