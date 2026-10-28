using GestionIntegral.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIntegral.Infrastructure.Data.Configurations;

public abstract class ConfiguracionBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : ConfiguracionBase
{
    private readonly string _tableName;
    private readonly string _primaryKeyColumn;
    private readonly int _descripcionMaxLength;

    protected ConfiguracionBaseConfiguration(string tableName, string primaryKeyColumn, int descripcionMaxLength = 100)
    {
        _tableName = tableName;
        _primaryKeyColumn = primaryKeyColumn;
        _descripcionMaxLength = descripcionMaxLength;
    }

    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(_tableName);

        var idProperty = typeof(T).GetProperty(_primaryKeyColumn)!;
        builder.HasKey(_primaryKeyColumn);

        builder.Property(idProperty.Name)
            .HasColumnName(_primaryKeyColumn)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Codigo)
            .HasColumnName("Codigo")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.Descripcion)
            .HasColumnName("Descripcion")
            .HasMaxLength(_descripcionMaxLength)
            .IsRequired();

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

        builder.HasIndex(e => e.Codigo)
            .IsUnique()
            .HasDatabaseName($"UQ_{_tableName}_Codigo");

        builder.HasOne(typeof(Usuario), "UsuarioCreacion")
            .WithMany()
            .HasForeignKey("IdUsuarioCreacion")
            .HasConstraintName($"FK_{_tableName}_UsuariosCreacion")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(typeof(Usuario), "UsuarioModificacion")
            .WithMany()
            .HasForeignKey("IdUsuarioUltimaModificacion")
            .HasConstraintName($"FK_{_tableName}_UsuariosModificacion")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
