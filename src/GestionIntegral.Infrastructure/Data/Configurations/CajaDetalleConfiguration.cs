using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class CajaDetalleConfiguration : IEntityTypeConfiguration<CajaDetalle>
{
    public void Configure(EntityTypeBuilder<CajaDetalle> builder)
    {
        builder.ToTable("CajaDetalle");

        builder.HasKey(e => e.IdCajaDetalle);

        builder.Property(e => e.IdCajaDetalle)
            .HasColumnName("IdCajaDetalle")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.IdCaja)
            .HasColumnName("IdCaja")
            .IsRequired();

        builder.Property(e => e.IdTipoMovimientoCaja)
            .HasColumnName("IdTipoMovimientoCaja")
            .IsRequired();

        builder.Property(e => e.IdMedioPago)
            .HasColumnName("IdMedioPago");

        builder.Property(e => e.IdMoneda)
            .HasColumnName("IdMoneda");

        builder.Property(e => e.FechaMovimiento)
            .HasColumnName("FechaMovimiento")
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(e => e.Concepto)
            .HasColumnName("Concepto")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Referencia)
            .HasColumnName("Referencia")
            .HasMaxLength(100);

        builder.Property(e => e.Importe)
            .HasColumnName("Importe")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.IdUsuarioCreacion)
            .HasColumnName("IdUsuarioCreacion")
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.HasOne(e => e.Caja)
            .WithMany(e => e.Detalles)
            .HasForeignKey(e => e.IdCaja)
            .HasConstraintName("FK_CajaDetalle_Cajas")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.TipoMovimientoCaja)
            .WithMany(e => e.CajaDetalles)
            .HasForeignKey(e => e.IdTipoMovimientoCaja)
            .HasConstraintName("FK_CajaDetalle_TipoMovimiento")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.MedioPago)
            .WithMany(e => e.CajaDetalles)
            .HasForeignKey(e => e.IdMedioPago)
            .HasConstraintName("FK_CajaDetalle_MedioPago")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Moneda)
            .WithMany(e => e.CajaDetalles)
            .HasForeignKey(e => e.IdMoneda)
            .HasConstraintName("FK_CajaDetalle_Moneda")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_CajaDetalle_UsuarioCreacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
