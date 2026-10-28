using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IListaPrecioDetalleRepository : IRepository<ListaPrecioDetalle>
{
    Task<bool> ExisteProductoEnListaAsync(int idListaPrecio, int idProducto, CancellationToken cancellationToken = default);
    Task<IEnumerable<ListaPrecioDetalle>> GetByListaPrecioAsync(int idListaPrecio, CancellationToken cancellationToken = default);
}
