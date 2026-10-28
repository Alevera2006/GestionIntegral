using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class ProvinciaRepository : ConfiguracionRepository<Provincia>, IProvinciaRepository
{
    public ProvinciaRepository(GestionIntegralDbContext context) : base(context) { }
}
