using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class CajaCabeceraConfiguration : IEntityTypeConfiguration<CajaCabecera>
{
    public void Configure(EntityTypeBuilder<CajaCabecera> builder)
    {
        builder.ToTable("CajaCabecera");

        builder.HasKey(e => e.IdCaja);

        builder.Property(e => e.IdCaja)
            .HasColumnName("IdCaja")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.FechaCaja)
            .HasColumnName("FechaCaja")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(e => e.IdUsuarioResponsable)
            .HasColumnName("IdUsuarioResponsable")
            .IsRequired();

        builder.Property(e => e.IdEstadoCaja)
            .HasColumnName("IdEstadoCaja")
            .IsRequired();

        builder.Property(e => e.SaldoInicial)
            .HasColumnName("SaldoInicial")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m)
            .IsRequired();

        builder.Property(e => e.SaldoFinal)
            .HasColumnName("SaldoFinal")
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.FechaApertura)
            .HasColumnName("FechaApertura")
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(e => e.FechaCierre)
            .HasColumnName("FechaCierre")
            .HasColumnType("datetime2");

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

        builder.HasOne(e => e.UsuarioResponsable)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioResponsable)
            .HasConstraintName("FK_CajaCabecera_Responsable")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.EstadoCaja)
            .WithMany(e => e.Cajas)
            .HasForeignKey(e => e.IdEstadoCaja)
            .HasConstraintName("FK_CajaCabecera_Estado")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_CajaCabecera_UsuarioCreacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
