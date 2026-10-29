using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class ClienteContactoConfiguration : IEntityTypeConfiguration<ClienteContacto>
{
    public void Configure(EntityTypeBuilder<ClienteContacto> builder)
    {
        builder.ToTable("ClienteContactos");

        builder.HasKey(e => new { e.IdCliente, e.IdContacto });

        builder.Property(e => e.IdCliente)
            .HasColumnName("IdCliente")
            .IsRequired();

        builder.Property(e => e.IdContacto)
            .HasColumnName("IdContacto")
            .IsRequired();

        builder.Property(e => e.EsPrincipal)
            .HasColumnName("EsPrincipal")
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne(e => e.Cliente)
            .WithMany()
            .HasForeignKey(e => e.IdCliente)
            .HasConstraintName("FK_ClienteContactos_Clientes")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Contacto)
            .WithMany(c => c.ClienteContactos)
            .HasForeignKey(e => e.IdContacto)
            .HasConstraintName("FK_ClienteContactos_Contactos")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
