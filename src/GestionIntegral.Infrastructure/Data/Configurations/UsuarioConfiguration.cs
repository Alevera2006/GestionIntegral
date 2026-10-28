using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(e => e.IdUsuario);

        builder.Property(e => e.IdUsuario)
            .HasColumnName("IdUsuario")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Nombre)
            .HasColumnName("Nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Apellido)
            .HasColumnName("Apellido")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.IdTipoUsuario)
            .HasColumnName("IdTipoUsuario")
            .IsRequired();

        builder.Property(e => e.UsuarioNombre)
            .HasColumnName("Usuario")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.ClaveHash)
            .HasColumnName("ClaveHash")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.Bloqueado)
            .HasColumnName("Bloqueado")
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
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(e => e.IdUsuarioUltimaModificacion)
            .HasColumnName("IdUsuarioUltimaModificacion");

        builder.Property(e => e.FechaUltimaModificacion)
            .HasColumnName("FechaUltimaModificacion")
            .HasColumnType("datetime2(3)");

        builder.Property(e => e.FechaBaja)
            .HasColumnName("FechaBaja")
            .HasColumnType("datetime2(3)");

        builder.HasIndex(e => e.UsuarioNombre)
            .IsUnique()
            .HasDatabaseName("UQ_Usuarios_Usuario");

        builder.HasOne(e => e.TipoUsuario)
            .WithMany(t => t.Usuarios)
            .HasForeignKey(e => e.IdTipoUsuario)
            .HasConstraintName("FK_Usuarios_TiposUsuario")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioCreacion)
            .WithMany(u => u.UsuariosCreados)
            .HasForeignKey(e => e.IdUsuarioCreacion)
            .HasConstraintName("FK_Usuarios_UsuarioCreacion")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UsuarioModificacion)
            .WithMany(u => u.UsuariosModificados)
            .HasForeignKey(e => e.IdUsuarioUltimaModificacion)
            .HasConstraintName("FK_Usuarios_UsuarioModificacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
