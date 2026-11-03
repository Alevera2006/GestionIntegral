using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IPagoProveedorRepository : IRepository<PagoProveedor>
{
    Task<IEnumerable<PagoProveedor>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PagoProveedor>> GetByProveedorAsync(int idProveedor, CancellationToken cancellationToken = default);
}
