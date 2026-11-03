using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class PagoProveedorRepository : Repository<PagoProveedor>, IPagoProveedorRepository
{
    public PagoProveedorRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<IEnumerable<PagoProveedor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await GetAllWithDetailsAsync(cancellationToken);
    }

    public async Task<IEnumerable<PagoProveedor>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Proveedor)
            .Include(p => p.MedioPago)
            .Where(p => p.Activo)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PagoProveedor>> GetByProveedorAsync(int idProveedor, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Proveedor)
            .Include(p => p.MedioPago)
            .Where(p => p.IdProveedor == idProveedor && p.Activo)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync(cancellationToken);
    }
}
