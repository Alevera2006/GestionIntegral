using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class EstadoFacturaRepository : ConfiguracionRepository<EstadoFactura>, IEstadoFacturaRepository
{
    public EstadoFacturaRepository(GestionIntegralDbContext context) : base(context) { }
}
