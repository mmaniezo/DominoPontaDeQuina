using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Context;

public class DominoDbContext(DbContextOptions<DominoDbContext> opcoes) : DbContext(opcoes)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Jogador> Jogadores => Set<Jogador>();
    public DbSet<Partida> Partidas => Set<Partida>();
    public DbSet<ParticipacaoPartida> ParticipacoesPartida => Set<ParticipacaoPartida>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        // Todas as entidades e relacionamentos sao mapeados pela Fluent API
        // (classes IEntityTypeConfiguration deste assembly).
        modelo.ApplyConfigurationsFromAssembly(typeof(DominoDbContext).Assembly);
        base.OnModelCreating(modelo);
    }
}
