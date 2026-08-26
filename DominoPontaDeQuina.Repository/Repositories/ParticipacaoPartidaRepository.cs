using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class ParticipacaoPartidaRepository(DominoDbContext contexto)
{
    public async Task<ParticipacaoPartida> AdicionarAsync(ParticipacaoPartida participacao, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(participacao);
        await contexto.ParticipacoesPartida.AddAsync(participacao, cancelamento);
        await contexto.SaveChangesAsync(cancelamento);
        return participacao;
    }

    public async Task AtualizarAsync(ParticipacaoPartida participacao, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(participacao);
        contexto.ParticipacoesPartida.Update(participacao);
        await contexto.SaveChangesAsync(cancelamento);
    }

    public async Task RemoverAsync(ParticipacaoPartida participacao, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(participacao);
        contexto.ParticipacoesPartida.Remove(participacao);
        await contexto.SaveChangesAsync(cancelamento);
    }

    public Task<ParticipacaoPartida?> ObterPorIdAsync(Guid id, CancellationToken cancelamento = default) =>
        contexto.ParticipacoesPartida
            .SingleOrDefaultAsync(participacao => participacao.Id == id, cancelamento);

    public Task<List<ParticipacaoPartida>> ListarPorPartidaAsync(Guid partidaId, CancellationToken cancelamento = default) =>
        contexto.ParticipacoesPartida
            .AsNoTracking()
            .Include(participacao => participacao.Jogador)
            .Where(participacao => participacao.PartidaId == partidaId)
            .OrderBy(participacao => participacao.Posicao)
            .ToListAsync(cancelamento);

    public Task<List<ParticipacaoPartida>> ListarPorJogadorAsync(Guid jogadorId, CancellationToken cancelamento = default) =>
        contexto.ParticipacoesPartida
            .AsNoTracking()
            .Include(participacao => participacao.Partida)
            .Where(participacao => participacao.JogadorId == jogadorId)
            .OrderByDescending(participacao => participacao.Partida.IniciadoEm)
            .ToListAsync(cancelamento);

    public Task<ParticipacaoPartida?> ObterVencedorDaPartidaAsync(Guid partidaId, CancellationToken cancelamento = default) =>
        contexto.ParticipacoesPartida
            .AsNoTracking()
            .Include(participacao => participacao.Jogador)
            .FirstOrDefaultAsync(
                participacao => participacao.PartidaId == partidaId && participacao.Vencedor,
                cancelamento);

    public Task<List<ParticipacaoPartida>> ListarMaioresPontuacoesAsync(int quantidade, CancellationToken cancelamento = default) =>
        contexto.ParticipacoesPartida
            .AsNoTracking()
            .Include(participacao => participacao.Jogador)
            .OrderByDescending(participacao => participacao.Pontuacao)
            .Take(quantidade)
            .ToListAsync(cancelamento);

    public async Task<double> ObterMediaPontuacaoPorJogadorAsync(Guid jogadorId, CancellationToken cancelamento = default)
    {
        var media = await contexto.ParticipacoesPartida
            .AsNoTracking()
            .Where(participacao => participacao.JogadorId == jogadorId)
            .AverageAsync(participacao => (double?)participacao.Pontuacao, cancelamento);

        return media ?? 0d;
    }

    public Task<bool> JogadorParticipouAsync(Guid partidaId, Guid jogadorId, CancellationToken cancelamento = default) =>
        contexto.ParticipacoesPartida
            .AsNoTracking()
            .AnyAsync(
                participacao => participacao.PartidaId == partidaId && participacao.JogadorId == jogadorId,
                cancelamento);
}
