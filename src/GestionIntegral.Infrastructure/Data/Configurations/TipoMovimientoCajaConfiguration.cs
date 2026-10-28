using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class TipoMovimientoCajaConfiguration : IEntityTypeConfiguration<TipoMovimientoCaja>
{
    public void Configure(EntityTypeBuilder<TipoMovimientoCaja> builder)
    {
        builder.ToTable("TiposMovimientoCaja");

        builder.HasKey(e => e.IdTipoMovimientoCaja);

        builder.Property(e => e.IdTipoMovimientoCaja)
            .HasColumnName("IdTipoMovimientoCaja")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Codigo)
            .HasColumnName("Codigo")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(e => e.Descripcion)
            .HasColumnName("Descripcion")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Signo)
            .HasColumnName("Signo")
            .IsRequired();

        builder.Property(e => e.Activo)
            .HasColumnName("Activo")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(e => e.IdUsuarioCreacion)
            .HasColumnName("IdUsuarioCreacion")
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.HasIndex(e => e.Codigo)
            .IsUnique()
            .HasDatabaseName("UQ_TiposMovimientoCaja_Codigo");

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_TiposMovimientoCaja_Usuarios")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
