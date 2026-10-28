using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class MedioPagoConfiguration : IEntityTypeConfiguration<MedioPago>
{
    public void Configure(EntityTypeBuilder<MedioPago> builder)
    {
        builder.ToTable("MediosPago");

        builder.HasKey(e => e.IdMedioPago);

        builder.Property(e => e.IdMedioPago)
            .HasColumnName("IdMedioPago")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Codigo)
            .HasColumnName("Codigo")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.Descripcion)
            .HasColumnName("Descripcion")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.EsEfectivo)
            .HasColumnName("EsEfectivo")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.RequiereReferencia)
            .HasColumnName("RequiereReferencia")
            .HasDefaultValue(false)
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
            .HasDatabaseName("UQ_MediosPago_Codigo");

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_MediosPago_Usuarios")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
