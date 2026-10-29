using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class DepositoRepository : ConfiguracionRepository<Deposito>, IDepositoRepository
{
    public DepositoRepository(GestionIntegralDbContext context) : base(context) { }
}
