using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominoPontaDeQuina.Repository.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> construtor)
    {
        construtor.ToTable("Usuarios");

        construtor.HasKey(usuario => usuario.Id);

        construtor.Property(usuario => usuario.Nome)
            .HasColumnName("Nome")
            .HasMaxLength(100)
            .IsRequired();

        construtor.Property(usuario => usuario.Email)
            .HasColumnName("Email")
            .HasMaxLength(200)
            .IsRequired();

        construtor.Property(usuario => usuario.HashSenha)
            .HasColumnName("HashSenha")
            .HasMaxLength(256)
            .IsRequired();

        construtor.Property(usuario => usuario.CriadoEm)
            .HasColumnName("CriadoEm")
            .HasColumnType("TEXT")
            .IsRequired();

        construtor.HasIndex(usuario => usuario.Email)
            .IsUnique();
    }
}
