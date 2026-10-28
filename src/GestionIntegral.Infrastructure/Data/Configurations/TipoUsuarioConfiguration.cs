using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class TipoUsuarioConfiguration : IEntityTypeConfiguration<TipoUsuario>
{
    public void Configure(EntityTypeBuilder<TipoUsuario> builder)
    {
        builder.ToTable("TiposUsuario");

        builder.HasKey(e => e.IdTipoUsuario);

        builder.Property(e => e.IdTipoUsuario)
            .HasColumnName("IdTipoUsuario")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Descripcion)
            .HasColumnName("Descripcion")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.IdUsuarioCreacion)
            .HasColumnName("IdUsuarioCreacion")
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(e => e.IdUsuarioUltimaModificacion)
            .HasColumnName("IdUsuarioUltimaModificacion");

        builder.Property(e => e.FechaUltimaModificacion)
            .HasColumnName("FechaUltimaModificacion")
            .HasColumnType("datetime2(3)");

        builder.Property(e => e.Activo)
            .HasColumnName("Activo")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(e => e.FechaBaja)
            .HasColumnName("FechaBaja")
            .HasColumnType("datetime2(3)");

        builder.HasIndex(e => e.Descripcion)
            .IsUnique()
            .HasDatabaseName("UQ_TiposUsuario_Descripcion");

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany(u => u.TiposUsuarioCreados)
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_TiposUsuario_UsuarioCreacion")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioModificacion)
            .WithMany(u => u.TiposUsuarioModificados)
            .HasForeignKey(e => e.IdUsuarioUltimaModificacion)
            .HasConstraintName("FK_TiposUsuario_UsuarioModificacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
