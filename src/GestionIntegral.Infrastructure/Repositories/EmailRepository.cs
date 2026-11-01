using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class EmailRepository : Repository<Email>, IEmailRepository
{
    public EmailRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<Email?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdEmail == id, cancellationToken);
    }

    public override async Task<IEnumerable<Email>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(x => x.Activo)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Email>> GetByOwnerAsync(string? tipoReferencia, int? idReferencia, CancellationToken cancellationToken = default)
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
