using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Domain.Interfaces;
using DominoPontaDeQuina.Service.Interfaces;

namespace DominoPontaDeQuina.Service.Services;

public class PartidaService(
    IPartidaRepository partidas,
    IParticipacaoPartidaRepository participacoes,
    IJogadorRepository jogadores) : IPartidaService
{
    public async Task<Partida> IniciarAsync(IReadOnlyList<Guid> jogadoresIds, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(jogadoresIds);

        if (jogadoresIds.Count < 2)
            throw new ArgumentException("Uma partida precisa de pelo menos dois jogadores.", nameof(jogadoresIds));

        if (jogadoresIds.Distinct().Count() != jogadoresIds.Count)
            throw new ArgumentException("O mesmo jogador nao pode ser informado mais de uma vez.", nameof(jogadoresIds));

        var partida = new Partida
        {
            Status = StatusJogo.EmAndamento
        };

        for (var posicao = 0; posicao < jogadoresIds.Count; posicao++)
        {
            var jogadorId = jogadoresIds[posicao];

            _ = await jogadores.ObterPorIdAsync(jogadorId, cancelamento)
                ?? throw new InvalidOperationException($"Jogador {jogadorId} nao encontrado.");

            partida.Participacoes.Add(new ParticipacaoPartida
            {
                PartidaId = partida.Id,
                JogadorId = jogadorId,
                Posicao = posicao + 1
            });
        }

        return await partidas.AdicionarAsync(partida, cancelamento);
    }

    public async Task<Partida> FinalizarAsync(Guid partidaId, Guid jogadorVencedorId, CancellationToken cancelamento = default)
    {
        var partida = await partidas.ObterPorIdAsync(partidaId, cancelamento)
            ?? throw new InvalidOperationException($"Partida {partidaId} nao encontrada.");

        if (partida.Status is not StatusJogo.EmAndamento)
            throw new InvalidOperationException("Somente uma partida em andamento pode ser finalizada.");

        var vencedor = await participacoes.ObterPorPartidaEJogadorAsync(partidaId, jogadorVencedorId, cancelamento)
            ?? throw new InvalidOperationException($"O jogador {jogadorVencedorId} nao participou da partida {partidaId}.");

        vencedor.Vencedor = true;
        await participacoes.AtualizarAsync(vencedor, cancelamento);

        partida.Status = StatusJogo.Finalizado;
        partida.FinalizadoEm = DateTime.UtcNow;
        await partidas.AtualizarAsync(partida, cancelamento);

        return partida;
    }

    public async Task RegistrarPontuacaoAsync(Guid partidaId, Guid jogadorId, int pontuacao, CancellationToken cancelamento = default)
    {
        if (pontuacao < 0)
            throw new ArgumentOutOfRangeException(nameof(pontuacao), "A pontuacao nao pode ser negativa.");

        var participacao = await participacoes.ObterPorPartidaEJogadorAsync(partidaId, jogadorId, cancelamento)
            ?? throw new InvalidOperationException($"O jogador {jogadorId} nao participou da partida {partidaId}.");

        participacao.Pontuacao = pontuacao;
        await participacoes.AtualizarAsync(participacao, cancelamento);
    }

    public Task<Partida?> ObterComParticipacoesAsync(Guid partidaId, CancellationToken cancelamento = default) =>
        partidas.ObterComParticipacoesAsync(partidaId, cancelamento);

    public Task<List<Partida>> ListarEmAndamentoAsync(CancellationToken cancelamento = default) =>
        partidas.ListarEmAndamentoAsync(cancelamento);

    public Task<List<Partida>> ListarPorJogadorAsync(Guid jogadorId, CancellationToken cancelamento = default) =>
        partidas.ListarPorJogadorAsync(jogadorId, cancelamento);

    public Task<List<ParticipacaoPartida>> ObterRankingAsync(int quantidade, CancellationToken cancelamento = default)
    {
        if (quantidade <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade do ranking deve ser maior que zero.");

        return participacoes.ListarMaioresPontuacoesAsync(quantidade, cancelamento);
    }
}
