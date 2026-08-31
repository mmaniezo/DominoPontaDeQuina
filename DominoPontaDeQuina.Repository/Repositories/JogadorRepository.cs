using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Domain.Interfaces;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class JogadorRepository(DominoDbContext contexto) : IJogadorRepository
{
    public async Task<Jogador> AdicionarAsync(Jogador jogador, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(jogador);
        await contexto.Jogadores.AddAsync(jogador, cancelamento);
        await contexto.SaveChangesAsync(cancelamento);
        return jogador;
    }

    public async Task AtualizarAsync(Jogador jogador, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(jogador);
        contexto.Jogadores.Update(jogador);
        await contexto.SaveChangesAsync(cancelamento);
    }

    public async Task RemoverAsync(Jogador jogador, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(jogador);
        contexto.Jogadores.Remove(jogador);
        await contexto.SaveChangesAsync(cancelamento);
    }

    public Task<Jogador?> ObterPorIdAsync(Guid id, CancellationToken cancelamento = default) =>
        contexto.Jogadores
            .SingleOrDefaultAsync(jogador => jogador.Id == id, cancelamento);

    public Task<Jogador?> ObterComUsuarioAsync(Guid id, CancellationToken cancelamento = default) =>
        contexto.Jogadores
            .AsNoTracking()
            .Include(jogador => jogador.Usuario)
            .SingleOrDefaultAsync(jogador => jogador.Id == id, cancelamento);

    public Task<List<Jogador>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancelamento = default) =>
        contexto.Jogadores
            .AsNoTracking()
            .Where(jogador => jogador.UsuarioId == usuarioId)
            .OrderBy(jogador => jogador.NomeExibicao)
            .ToListAsync(cancelamento);

    public Task<List<Jogador>> BuscarPorNomeExibicaoAsync(string termo, CancellationToken cancelamento = default) =>
        contexto.Jogadores
            .AsNoTracking()
            .Where(jogador => jogador.NomeExibicao.Contains(termo))
            .OrderBy(jogador => jogador.NomeExibicao)
            .ToListAsync(cancelamento);

    public Task<List<Jogador>> ListarComParticipacoesAsync(CancellationToken cancelamento = default) =>
        contexto.Jogadores
            .AsNoTracking()
            .Include(jogador => jogador.Participacoes)
                .ThenInclude(participacao => participacao.Partida)
            .OrderBy(jogador => jogador.NomeExibicao)
            .ToListAsync(cancelamento);

    public Task<int> ContarVitoriasAsync(Guid jogadorId, CancellationToken cancelamento = default) =>
        contexto.ParticipacoesPartida
            .AsNoTracking()
            .CountAsync(participacao => participacao.JogadorId == jogadorId && participacao.Vencedor, cancelamento);

    public Task<int> ObterPontuacaoTotalAsync(Guid jogadorId, CancellationToken cancelamento = default) =>
        contexto.ParticipacoesPartida
            .AsNoTracking()
            .Where(participacao => participacao.JogadorId == jogadorId)
            .SumAsync(participacao => participacao.Pontuacao, cancelamento);
}
