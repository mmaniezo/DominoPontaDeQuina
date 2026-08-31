using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Domain.Interfaces;

public interface IPartidaRepository
{
    Task<Partida> AdicionarAsync(Partida partida, CancellationToken cancelamento = default);

    Task AtualizarAsync(Partida partida, CancellationToken cancelamento = default);

    Task RemoverAsync(Partida partida, CancellationToken cancelamento = default);

    Task<Partida?> ObterPorIdAsync(Guid id, CancellationToken cancelamento = default);

    Task<Partida?> ObterComParticipacoesAsync(Guid id, CancellationToken cancelamento = default);

    Task<List<Partida>> ListarPorStatusAsync(StatusJogo status, CancellationToken cancelamento = default);

    Task<List<Partida>> ListarEmAndamentoAsync(CancellationToken cancelamento = default);

    Task<List<Partida>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancelamento = default);

    Task<List<Partida>> ListarPorJogadorAsync(Guid jogadorId, CancellationToken cancelamento = default);

    Task<Partida?> ObterUltimaFinalizadaAsync(CancellationToken cancelamento = default);

    Task<int> ContarPorStatusAsync(StatusJogo status, CancellationToken cancelamento = default);
}
