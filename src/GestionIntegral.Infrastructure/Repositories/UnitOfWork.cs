using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GestionIntegralDbContext _context;

    public IUsuarioRepository Usuarios { get; }
    public ITipoUsuarioRepository TiposUsuario { get; }

    public UnitOfWork(GestionIntegralDbContext context)
    {
        _context = context;
        Usuarios = new UsuarioRepository(context);
        TiposUsuario = new TipoUsuarioRepository(context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
