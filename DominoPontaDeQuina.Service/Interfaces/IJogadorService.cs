using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Service.Interfaces;

public interface IJogadorService
{
    Task<Jogador> CadastrarAsync(Guid usuarioId, string nomeExibicao, CancellationToken cancelamento = default);

    Task<List<Jogador>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancelamento = default);

    Task<Jogador?> ObterComUsuarioAsync(Guid jogadorId, CancellationToken cancelamento = default);

    Task<int> ContarVitoriasAsync(Guid jogadorId, CancellationToken cancelamento = default);

    Task<int> ObterPontuacaoTotalAsync(Guid jogadorId, CancellationToken cancelamento = default);
}
