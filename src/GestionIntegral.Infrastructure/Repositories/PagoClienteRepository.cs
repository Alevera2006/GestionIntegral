using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class PagoClienteRepository : Repository<PagoCliente>, IPagoClienteRepository
{
    public PagoClienteRepository(GestionIntegralDbContext context) : base(context) { }

    public async Task<IEnumerable<PagoCliente>> GetByClienteAsync(int idCliente, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.MedioPago)
            .Include(p => p.Factura)
            .Where(p => p.IdCliente == idCliente && p.Activo)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync(cancellationToken);
    }
}
