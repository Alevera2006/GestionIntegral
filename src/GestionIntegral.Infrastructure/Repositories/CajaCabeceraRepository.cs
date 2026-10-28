using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class CajaCabeceraRepository : Repository<CajaCabecera>, ICajaCabeceraRepository
{
    public CajaCabeceraRepository(GestionIntegralDbContext context) : base(context) { }
}
