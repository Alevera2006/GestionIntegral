using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface ITipoUsuarioRepository : IRepository<TipoUsuario>
{
    Task<TipoUsuario?> GetByDescripcionAsync(string descripcion, CancellationToken cancellationToken = default);
}
