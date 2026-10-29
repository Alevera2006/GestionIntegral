using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;

namespace GestionIntegral.Infrastructure.Repositories;

public class ContactoRepository : Repository<Contacto>, IContactoRepository
{
    public ContactoRepository(GestionIntegralDbContext context) : base(context) { }
}
