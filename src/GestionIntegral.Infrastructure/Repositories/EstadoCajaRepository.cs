using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class EstadoCajaRepository : ConfiguracionRepository<EstadoCaja>, IEstadoCajaRepository
{
    public EstadoCajaRepository(GestionIntegralDbContext context) : base(context) { }
}
