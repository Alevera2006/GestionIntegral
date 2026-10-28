using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class MotivoAnulacionRepository : ConfiguracionRepository<MotivoAnulacion>, IMotivoAnulacionRepository
{
    public MotivoAnulacionRepository(GestionIntegralDbContext context) : base(context) { }
}
