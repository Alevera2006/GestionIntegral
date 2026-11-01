using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class TelefonoRepository : Repository<Telefono>, ITelefonoRepository
{
    public TelefonoRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<Telefono?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdTelefono == id, cancellationToken);
    }

    public override async Task<IEnumerable<Telefono>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(x => x.Activo)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Telefono>> GetByOwnerAsync(string? tipoReferencia, int? idReferencia, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .AsNoTracking()
            .Where(x => x.Activo);

        if (!string.IsNullOrWhiteSpace(tipoReferencia))
            query = query.Where(x => x.TipoReferencia == tipoReferencia);

        if (idReferencia.HasValue)
            query = query.Where(x => x.IdReferencia == idReferencia.Value);

        return await query.ToListAsync(cancellationToken);
    }
}
