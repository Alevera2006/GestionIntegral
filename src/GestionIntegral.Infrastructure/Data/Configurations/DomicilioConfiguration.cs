using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class DomicilioConfiguration : IEntityTypeConfiguration<Domicilio>
{
    public void Configure(EntityTypeBuilder<Domicilio> builder)
    {
        builder.ToTable("Domicilios", t =>
        {
            t.HasCheckConstraint("CK_Domicilios_TipoReferencia", "TipoReferencia IN ('C','P')");
            t.HasCheckConstraint("CK_Domicilios_IdReferencia", "IdReferencia > 0");
        });

        builder.HasKey(e => e.IdDomicilio);

        builder.Property(e => e.IdDomicilio)
            .HasColumnName("IdDomicilio")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.TipoReferencia)
            .HasColumnName("TipoReferencia")
            .HasMaxLength(1)
            .IsRequired();

        builder.Property(e => e.IdReferencia)
            .HasColumnName("IdReferencia")
            .IsRequired();

        builder.Property(e => e.Direccion)
            .HasColumnName("Domicilio")
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(e => e.Ciudad)
            .HasColumnName("Ciudad")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Localidad)
            .HasColumnName("Localidad")
            .HasMaxLength(100);

        builder.Property(e => e.IdProvincia)
            .HasColumnName("IdProvincia")
            .IsRequired();

        builder.Property(e => e.CodigoPostal)
            .HasColumnName("CodigoPostal")
            .HasMaxLength(20);

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

        builder.HasIndex(e => new { e.TipoReferencia, e.IdReferencia }).HasDatabaseName("IX_Domicilios_TipoReferencia_IdReferencia");
        builder.HasIndex(e => e.IdProvincia).HasDatabaseName("IX_Domicilios_IdProvincia");

        builder.HasOne(e => e.Provincia)
            .WithMany()
            .HasForeignKey(e => e.IdProvincia)
            .HasConstraintName("FK_Domicilios_Provincias")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_Domicilios_UsuariosCreacion")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioModificacion)
            .WithMany()
            .HasForeignKey(e => e.IdUsuarioUltimaModificacion)
            .HasConstraintName("FK_Domicilios_UsuariosModificacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
