using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class TipoMovimientoStockRepository : ConfiguracionRepository<TipoMovimientoStock>, ITipoMovimientoStockRepository
{
    public TipoMovimientoStockRepository(GestionIntegralDbContext context) : base(context) { }
}
