using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(GestionIntegralDbContext context) : base(context) { }

    public override async Task<Usuario?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(u => u.TipoUsuario)
            .FirstOrDefaultAsync(u => u.IdUsuario == id, cancellationToken);
    }

    public override async Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(u => u.TipoUsuario)
            .ToListAsync(cancellationToken);
    }

    public async Task<Usuario?> GetByUsuarioNombreAsync(string usuarioNombre, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(u => u.TipoUsuario)
            .FirstOrDefaultAsync(u => u.UsuarioNombre == usuarioNombre, cancellationToken);
    }

    public async Task<bool> ExisteUsuarioAsync(string usuarioNombre, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(u => u.UsuarioNombre == usuarioNombre, cancellationToken);
    }
}
