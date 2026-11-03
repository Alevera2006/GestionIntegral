using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Repositories;

public interface IFacturaRepository : IRepository<Factura>
{
    Task<IEnumerable<Factura>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<Factura?> GetByIdLongAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExisteNumeroComprobanteAsync(string numeroComprobante, CancellationToken cancellationToken = default);
}
