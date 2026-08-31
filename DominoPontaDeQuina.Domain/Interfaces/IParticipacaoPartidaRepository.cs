using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Domain.Interfaces;

public interface IParticipacaoPartidaRepository
{
    Task<ParticipacaoPartida> AdicionarAsync(ParticipacaoPartida participacao, CancellationToken cancelamento = default);

    Task AtualizarAsync(ParticipacaoPartida participacao, CancellationToken cancelamento = default);

    Task RemoverAsync(ParticipacaoPartida participacao, CancellationToken cancelamento = default);

    Task<ParticipacaoPartida?> ObterPorIdAsync(Guid id, CancellationToken cancelamento = default);

    Task<List<ParticipacaoPartida>> ListarPorPartidaAsync(Guid partidaId, CancellationToken cancelamento = default);

    Task<ParticipacaoPartida?> ObterPorPartidaEJogadorAsync(Guid partidaId, Guid jogadorId, CancellationToken cancelamento = default);

    Task<List<ParticipacaoPartida>> ListarPorJogadorAsync(Guid jogadorId, CancellationToken cancelamento = default);

    Task<ParticipacaoPartida?> ObterVencedorDaPartidaAsync(Guid partidaId, CancellationToken cancelamento = default);

    Task<List<ParticipacaoPartida>> ListarMaioresPontuacoesAsync(int quantidade, CancellationToken cancelamento = default);

    Task<double> ObterMediaPontuacaoPorJogadorAsync(Guid jogadorId, CancellationToken cancelamento = default);

    Task<bool> JogadorParticipouAsync(Guid partidaId, Guid jogadorId, CancellationToken cancelamento = default);
}
