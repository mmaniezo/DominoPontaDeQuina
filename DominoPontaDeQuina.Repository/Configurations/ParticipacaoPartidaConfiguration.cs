using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominoPontaDeQuina.Repository.Configurations;

public class ParticipacaoPartidaConfiguration : IEntityTypeConfiguration<ParticipacaoPartida>
{
    public void Configure(EntityTypeBuilder<ParticipacaoPartida> construtor)
    {
        construtor.ToTable("ParticipacoesPartida");

        construtor.HasKey(participacao => participacao.Id);

        construtor.Property(participacao => participacao.PartidaId)
            .HasColumnName("PartidaId")
            .IsRequired();

        construtor.Property(participacao => participacao.JogadorId)
            .HasColumnName("JogadorId")
            .IsRequired();

        construtor.Property(participacao => participacao.Posicao)
            .HasColumnName("Posicao")
            .IsRequired();

        construtor.Property(participacao => participacao.Pontuacao)
            .HasColumnName("Pontuacao")
            .IsRequired();

        construtor.Property(participacao => participacao.Vencedor)
            .HasColumnName("Vencedor")
            .IsRequired();

        construtor.HasIndex(participacao => new { participacao.PartidaId, participacao.JogadorId })
            .IsUnique();
    }
}
