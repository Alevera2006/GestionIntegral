using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class EstadoCajaConfiguration : IEntityTypeConfiguration<EstadoCaja>
{
    public void Configure(EntityTypeBuilder<EstadoCaja> builder)
    {
        builder.ToTable("EstadosCaja");

        builder.HasKey(e => e.IdEstadoCaja);

        builder.Property(e => e.IdEstadoCaja)
            .HasColumnName("IdEstadoCaja")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Codigo)
            .HasColumnName("Codigo")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.Descripcion)
            .HasColumnName("Descripcion")
            .HasMaxLength(100)
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
            .HasDatabaseName("UQ_EstadosCaja_Codigo");

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_EstadosCaja_Usuarios")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
