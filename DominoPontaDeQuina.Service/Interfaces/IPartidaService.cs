using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Service.Interfaces;

public interface IPartidaService
{
    Task<Partida> IniciarAsync(IReadOnlyList<Guid> jogadoresIds, CancellationToken cancelamento = default);

    Task<Partida> FinalizarAsync(Guid partidaId, Guid jogadorVencedorId, CancellationToken cancelamento = default);

    Task<Partida?> ObterComParticipacoesAsync(Guid partidaId, CancellationToken cancelamento = default);

    Task<List<Partida>> ListarEmAndamentoAsync(CancellationToken cancelamento = default);

    Task<List<Partida>> ListarPorJogadorAsync(Guid jogadorId, CancellationToken cancelamento = default);

    Task RegistrarPontuacaoAsync(Guid partidaId, Guid jogadorId, int pontuacao, CancellationToken cancelamento = default);

    Task<List<ParticipacaoPartida>> ObterRankingAsync(int quantidade, CancellationToken cancelamento = default);
}
