using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Domain.Interfaces;

public interface IJogadorRepository
{
    Task<Jogador> AdicionarAsync(Jogador jogador, CancellationToken cancelamento = default);

    Task AtualizarAsync(Jogador jogador, CancellationToken cancelamento = default);

    Task RemoverAsync(Jogador jogador, CancellationToken cancelamento = default);

    Task<Jogador?> ObterPorIdAsync(Guid id, CancellationToken cancelamento = default);

    Task<Jogador?> ObterComUsuarioAsync(Guid id, CancellationToken cancelamento = default);

    Task<List<Jogador>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancelamento = default);

    Task<List<Jogador>> BuscarPorNomeExibicaoAsync(string termo, CancellationToken cancelamento = default);

    Task<List<Jogador>> ListarComParticipacoesAsync(CancellationToken cancelamento = default);

    Task<int> ContarVitoriasAsync(Guid jogadorId, CancellationToken cancelamento = default);

    Task<int> ObterPontuacaoTotalAsync(Guid jogadorId, CancellationToken cancelamento = default);
}
