using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IListaPrecioRepository : IRepository<ListaPrecio>
{
    Task<ListaPrecio?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<ListaPrecio?> GetByIdWithDetallesAsync(int id, CancellationToken cancellationToken = default);
}
