using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class EstadoPedidoRepository : ConfiguracionRepository<EstadoPedido>, IEstadoPedidoRepository
{
    public EstadoPedidoRepository(GestionIntegralDbContext context) : base(context) { }
}
