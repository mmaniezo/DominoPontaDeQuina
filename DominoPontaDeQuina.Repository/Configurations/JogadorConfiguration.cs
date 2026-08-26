using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominoPontaDeQuina.Repository.Configurations;

public class JogadorConfiguration : IEntityTypeConfiguration<Jogador>
{
    public void Configure(EntityTypeBuilder<Jogador> construtor)
    {
        construtor.ToTable("Jogadores");

        construtor.HasKey(jogador => jogador.Id);

        construtor.Property(jogador => jogador.NomeExibicao)
            .HasColumnName("NomeExibicao")
            .HasMaxLength(50)
            .IsRequired();

        construtor.Property(jogador => jogador.UsuarioId)
            .HasColumnName("UsuarioId")
            .IsRequired();

        construtor.HasOne(jogador => jogador.Usuario)
            .WithMany(usuario => usuario.Jogadores)
            .HasForeignKey(jogador => jogador.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        construtor.HasMany(jogador => jogador.Participacoes)
            .WithOne(participacao => participacao.Jogador)
            .HasForeignKey(participacao => participacao.JogadorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
