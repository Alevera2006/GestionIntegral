using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class DomicilioRepository : Repository<Domicilio>, IDomicilioRepository
{
    public DomicilioRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<Domicilio?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Provincia)
            .FirstOrDefaultAsync(x => x.IdDomicilio == id, cancellationToken);
    }

    public override async Task<IEnumerable<Domicilio>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Provincia)
            .Where(x => x.Activo)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domicilio>> GetByOwnerAsync(string? tipoReferencia, int? idReferencia, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .AsNoTracking()
            .Include(x => x.Provincia)
            .Where(x => x.Activo);

        if (!string.IsNullOrWhiteSpace(tipoReferencia))
            query = query.Where(x => x.TipoReferencia == tipoReferencia);

        if (idReferencia.HasValue)
            query = query.Where(x => x.IdReferencia == idReferencia.Value);

        return await query.ToListAsync(cancellationToken);
    }
}
