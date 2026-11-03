using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class ValorCarteraRepository : Repository<ValorCartera>, IValorCarteraRepository
{
    public ValorCarteraRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<IEnumerable<ValorCartera>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await GetAllWithDetailsAsync(cancellationToken);
    }

    public async Task<IEnumerable<ValorCartera>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(v => v.Cliente)
            .Include(v => v.PagoCliente)
            .Where(v => v.Activo)
            .OrderByDescending(v => v.FechaVencimiento)
            .ToListAsync(cancellationToken);
    }
}
