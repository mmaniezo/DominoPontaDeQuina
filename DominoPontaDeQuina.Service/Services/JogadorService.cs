using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Domain.Interfaces;
using DominoPontaDeQuina.Service.Interfaces;

namespace DominoPontaDeQuina.Service.Services;

public class JogadorService(IJogadorRepository jogadores, IUsuarioRepository usuarios) : IJogadorService
{
    public async Task<Jogador> CadastrarAsync(Guid usuarioId, string nomeExibicao, CancellationToken cancelamento = default)
    {
        if (string.IsNullOrWhiteSpace(nomeExibicao))
            throw new ArgumentException("O nome de exibicao e obrigatorio.", nameof(nomeExibicao));

        var usuario = await usuarios.ObterPorIdAsync(usuarioId, cancelamento)
            ?? throw new InvalidOperationException($"Usuario {usuarioId} nao encontrado.");

        var jogador = new Jogador
        {
            UsuarioId = usuario.Id,
            NomeExibicao = nomeExibicao.Trim()
        };

        return await jogadores.AdicionarAsync(jogador, cancelamento);
    }

    public Task<List<Jogador>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancelamento = default) =>
        jogadores.ListarPorUsuarioAsync(usuarioId, cancelamento);

    public Task<Jogador?> ObterComUsuarioAsync(Guid jogadorId, CancellationToken cancelamento = default) =>
        jogadores.ObterComUsuarioAsync(jogadorId, cancelamento);

    public Task<int> ContarVitoriasAsync(Guid jogadorId, CancellationToken cancelamento = default) =>
        jogadores.ContarVitoriasAsync(jogadorId, cancelamento);

    public Task<int> ObterPontuacaoTotalAsync(Guid jogadorId, CancellationToken cancelamento = default) =>
        jogadores.ObterPontuacaoTotalAsync(jogadorId, cancelamento);
}
