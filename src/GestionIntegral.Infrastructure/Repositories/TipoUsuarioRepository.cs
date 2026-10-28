using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class TipoUsuarioRepository : Repository<TipoUsuario>, ITipoUsuarioRepository
{
    public TipoUsuarioRepository(GestionIntegralDbContext context) : base(context) { }

    public async Task<TipoUsuario?> GetByDescripcionAsync(string descripcion, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Descripcion == descripcion, cancellationToken);
    }
}
