using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class TipoMovimientoCajaRepository : ConfiguracionRepository<TipoMovimientoCaja>, ITipoMovimientoCajaRepository
{
    public TipoMovimientoCajaRepository(GestionIntegralDbContext context) : base(context) { }
}
