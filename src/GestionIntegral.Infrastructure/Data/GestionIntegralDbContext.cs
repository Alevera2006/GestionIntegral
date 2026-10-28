using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionIntegral.Infrastructure.Data;

public class GestionIntegralDbContext : DbContext
{
    public GestionIntegralDbContext(DbContextOptions<GestionIntegralDbContext> options) : base(options) { }

    public DbSet<TipoUsuario> TiposUsuario => Set<TipoUsuario>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GestionIntegralDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
