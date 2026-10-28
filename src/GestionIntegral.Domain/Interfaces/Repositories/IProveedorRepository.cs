using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IProveedorRepository : IRepository<Proveedor>
{
    Task<Proveedor?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default);
}
