using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(e => e.IdCliente);

        builder.Property(e => e.IdCliente)
            .HasColumnName("IdCliente")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Codigo)
            .HasColumnName("Codigo")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.RazonSocial)
            .HasColumnName("RazonSocial")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.NombreFantasia)
            .HasColumnName("NombreFantasia")
            .HasMaxLength(150);

        builder.Property(e => e.CUIT)
            .HasColumnName("CUIT")
            .HasMaxLength(13);

        builder.Property(e => e.Email)
            .HasColumnName("Email")
            .HasMaxLength(150);

        builder.Property(e => e.Telefono)
            .HasColumnName("Telefono")
            .HasMaxLength(50);

        builder.Property(e => e.IdCondicionPago)
            .HasColumnName("IdCondicionPago");

        builder.Property(e => e.Observaciones)
            .HasColumnName("Observaciones")
            .HasMaxLength(500);

        builder.Property(e => e.Activo)
            .HasColumnName("Activo")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(e => e.FechaBaja)
            .HasColumnName("FechaBaja")
            .HasColumnType("datetime2");

        builder.Property(e => e.IdUsuarioCreacion)
            .HasColumnName("IdUsuarioCreacion")
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(e => e.IdUsuarioUltimaModificacion)
            .HasColumnName("IdUsuarioUltimaModificacion");

        builder.Property(e => e.FechaUltimaModificacion)
            .HasColumnName("FechaUltimaModificacion")
            .HasColumnType("datetime2");

        builder.HasIndex(e => e.Codigo)
            .IsUnique()
            .HasDatabaseName("UQ_Clientes_Codigo");

        builder.HasOne(e => e.CondicionPago)
            .WithMany(c => c.Clientes)
            .HasForeignKey(e => e.IdCondicionPago)
            .HasConstraintName("FK_Clientes_CondicionesPago")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_Clientes_UsuariosCreacion")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioModificacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioUltimaModificacion)
            .HasConstraintName("FK_Clientes_UsuariosModificacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
