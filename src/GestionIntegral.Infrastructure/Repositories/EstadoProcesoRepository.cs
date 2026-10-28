using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class EstadoProcesoRepository : ConfiguracionRepository<EstadoProceso>, IEstadoProcesoRepository
{
    public EstadoProcesoRepository(GestionIntegralDbContext context) : base(context) { }
}
