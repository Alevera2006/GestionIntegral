using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class TipoComprobanteRepository : ConfiguracionRepository<TipoComprobante>, ITipoComprobanteRepository
{
    public TipoComprobanteRepository(GestionIntegralDbContext context) : base(context) { }
}
