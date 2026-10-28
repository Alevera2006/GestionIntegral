using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class MonedaConfiguration : IEntityTypeConfiguration<Moneda>
{
    public void Configure(EntityTypeBuilder<Moneda> builder)
    {
        builder.ToTable("Monedas");

        builder.HasKey(e => e.IdMoneda);

        builder.Property(e => e.IdMoneda)
            .HasColumnName("IdMoneda")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Codigo)
            .HasColumnName("Codigo")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(e => e.Descripcion)
            .HasColumnName("Descripcion")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Simbolo)
            .HasColumnName("Simbolo")
            .HasMaxLength(10);

        builder.Property(e => e.CantidadDecimales)
            .HasColumnName("CantidadDecimales")
            .HasDefaultValue((byte)2)
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
            .HasDatabaseName("UQ_Monedas_Codigo");

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_Monedas_Usuarios")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
