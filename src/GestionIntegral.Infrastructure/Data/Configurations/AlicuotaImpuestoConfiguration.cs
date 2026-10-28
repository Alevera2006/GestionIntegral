using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class AlicuotaImpuestoConfiguration : IEntityTypeConfiguration<AlicuotaImpuesto>
{
    public void Configure(EntityTypeBuilder<AlicuotaImpuesto> builder)
    {
        builder.ToTable("AlicuotasImpuesto");

        builder.HasKey(e => e.IdAlicuotaImpuesto);

        builder.Property(e => e.IdAlicuotaImpuesto)
            .HasColumnName("IdAlicuotaImpuesto")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.IdTipoImpuesto)
            .HasColumnName("IdTipoImpuesto")
            .IsRequired();

        builder.Property(e => e.Codigo)
            .HasColumnName("Codigo")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.Descripcion)
            .HasColumnName("Descripcion")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Porcentaje)
            .HasColumnName("Porcentaje")
            .HasColumnType("decimal(8,4)")
            .IsRequired();

        builder.Property(e => e.FechaDesde)
            .HasColumnName("FechaDesde")
            .HasColumnType("date");

        builder.Property(e => e.FechaHasta)
            .HasColumnName("FechaHasta")
            .HasColumnType("date");

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
            .HasDatabaseName("UQ_AlicuotasImpuesto_Codigo");

        builder.HasOne(e => e.TipoImpuesto)
            .WithMany(t => t.Alicuotas)
            .HasForeignKey(e => e.IdTipoImpuesto)
            .HasConstraintName("FK_AlicuotasImpuesto_TiposImpuesto")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_AlicuotasImpuesto_UsuariosCreacion")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioModificacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioUltimaModificacion)
            .HasConstraintName("FK_AlicuotasImpuesto_UsuariosModificacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
