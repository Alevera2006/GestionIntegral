using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByUsuarioNombreAsync(string usuarioNombre, CancellationToken cancellationToken = default);
    Task<bool> ExisteUsuarioAsync(string usuarioNombre, CancellationToken cancellationToken = default);
}
