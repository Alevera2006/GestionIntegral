using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class MovimientoStockConfiguration : IEntityTypeConfiguration<MovimientoStock>
{
    public void Configure(EntityTypeBuilder<MovimientoStock> builder)
    {
        builder.ToTable("MovimientosStock");

        builder.HasKey(e => e.IdMovimientoStock);

        builder.Property(e => e.IdMovimientoStock)
            .HasColumnName("IdMovimientoStock")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.IdDeposito)
            .HasColumnName("IdDeposito")
            .IsRequired();

        builder.Property(e => e.IdTipoMovimientoStock)
            .HasColumnName("IdTipoMovimientoStock")
            .IsRequired();

        builder.Property(e => e.FechaMovimiento)
            .HasColumnName("FechaMovimiento")
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(e => e.IdUsuarioResponsable)
            .HasColumnName("IdUsuarioResponsable")
            .IsRequired();

        builder.Property(e => e.Referencia)
            .HasColumnName("Referencia")
            .HasMaxLength(100);

        builder.Property(e => e.Observaciones)
            .HasColumnName("Observaciones")
            .HasMaxLength(500);

        builder.Property(e => e.IdUsuarioCreacion)
            .HasColumnName("IdUsuarioCreacion")
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.HasOne(e => e.Deposito)
            .WithMany(d => d.MovimientosStock)
            .HasForeignKey(e => e.IdDeposito)
            .HasConstraintName("FK_MovimientosStock_Depositos")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.TipoMovimientoStock)
            .WithMany(t => t.MovimientosStock)
            .HasForeignKey(e => e.IdTipoMovimientoStock)
            .HasConstraintName("FK_MovimientosStock_Tipo")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioResponsable)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioResponsable)
            .HasConstraintName("FK_MovimientosStock_Responsable")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_MovimientosStock_UsuarioCreacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
