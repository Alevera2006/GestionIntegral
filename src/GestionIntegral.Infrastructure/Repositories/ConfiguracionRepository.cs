using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Repositories;

public class ConfiguracionRepository<T> : Repository<T>, IConfiguracionRepository<T> where T : class
{
    public ConfiguracionRepository(GestionIntegralDbContext context) : base(context) { }

    public virtual async Task<T?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        var propiedad = typeof(T).GetProperty("Codigo");
        if (propiedad is null)
            throw new InvalidOperationException($"La entidad {typeof(T).Name} no tiene propiedad Codigo.");

        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(e => EF.Property<string>(e, "Codigo") == codigo, cancellationToken);
    }

    public virtual async Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(e => EF.Property<string>(e, "Codigo") == codigo, cancellationToken);
    }
}
