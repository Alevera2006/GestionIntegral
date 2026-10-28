using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class MonedaRepository : ConfiguracionRepository<Moneda>, IMonedaRepository
{
    public MonedaRepository(GestionIntegralDbContext context) : base(context) { }
}
