using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class ClienteContactoRepository : Repository<ClienteContacto>, IClienteContactoRepository
{
    public ClienteContactoRepository(GestionIntegralDbContext context) : base(context) { }

    public async Task<IEnumerable<ClienteContacto>> GetByClienteAsync(int idCliente, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.IdCliente == idCliente)
            .ToListAsync(cancellationToken);
    }

    public async Task<ClienteContacto?> GetByIdsAsync(int idCliente, int idContacto, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCliente == idCliente && c.IdContacto == idContacto, cancellationToken);
    }
}
