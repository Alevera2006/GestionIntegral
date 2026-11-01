using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class TelefonoConfiguration : IEntityTypeConfiguration<Telefono>
{
    public void Configure(EntityTypeBuilder<Telefono> builder)
    {
        builder.ToTable("Telefonos", t =>
        {
            t.HasCheckConstraint("CK_Telefonos_TipoReferencia", "TipoReferencia IN ('C','P')");
            t.HasCheckConstraint("CK_Telefonos_IdReferencia", "IdReferencia > 0");
        });

        builder.HasKey(e => e.IdTelefono);

        builder.Property(e => e.IdTelefono)
            .HasColumnName("IdTelefono")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.TipoReferencia)
            .HasColumnName("TipoReferencia")
            .HasMaxLength(1)
            .IsRequired();

        builder.Property(e => e.IdReferencia)
            .HasColumnName("IdReferencia")
            .IsRequired();

        builder.Property(e => e.NumeroTelefono)
            .HasColumnName("NumeroTelefono")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.TipoTelefono)
            .HasColumnName("TipoTelefono")
            .HasMaxLength(7)
            .IsRequired();

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

        builder.HasIndex(e => new { e.TipoReferencia, e.IdReferencia }).HasDatabaseName("IX_Telefonos_TipoReferencia_IdReferencia");

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_Telefonos_UsuariosCreacion")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioModificacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioUltimaModificacion)
            .HasConstraintName("FK_Telefonos_UsuariosModificacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
