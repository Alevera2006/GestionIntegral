using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class MedioPagoRepository : ConfiguracionRepository<MedioPago>, IMedioPagoRepository
{
    public MedioPagoRepository(GestionIntegralDbContext context) : base(context) { }
}
