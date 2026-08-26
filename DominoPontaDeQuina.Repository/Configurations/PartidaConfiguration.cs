using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominoPontaDeQuina.Repository.Configurations;

public class PartidaConfiguration : IEntityTypeConfiguration<Partida>
{
    public void Configure(EntityTypeBuilder<Partida> construtor)
    {
        construtor.ToTable("Partidas");

        construtor.HasKey(partida => partida.Id);

        construtor.Property(partida => partida.IniciadoEm)
            .HasColumnName("IniciadoEm")
            .HasColumnType("TEXT")
            .IsRequired();

        construtor.Property(partida => partida.FinalizadoEm)
            .HasColumnName("FinalizadoEm")
            .HasColumnType("TEXT");

        construtor.Property(partida => partida.Status)
            .HasColumnName("Status")
            .HasConversion<int>()
            .IsRequired();

        construtor.HasMany(partida => partida.Participacoes)
            .WithOne(participacao => participacao.Partida)
            .HasForeignKey(participacao => participacao.PartidaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
