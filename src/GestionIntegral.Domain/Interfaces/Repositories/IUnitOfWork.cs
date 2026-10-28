namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUsuarioRepository Usuarios { get; }
    ITipoUsuarioRepository TiposUsuario { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
