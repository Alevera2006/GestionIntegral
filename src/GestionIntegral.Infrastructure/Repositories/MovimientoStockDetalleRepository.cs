using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class MovimientoStockDetalleRepository : Repository<MovimientoStockDetalle>, IMovimientoStockDetalleRepository
{
    public MovimientoStockDetalleRepository(GestionIntegralDbContext context) : base(context) { }
}
