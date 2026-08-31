using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario> AdicionarAsync(Usuario usuario, CancellationToken cancelamento = default);

    Task AtualizarAsync(Usuario usuario, CancellationToken cancelamento = default);

    Task RemoverAsync(Usuario usuario, CancellationToken cancelamento = default);

    Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancelamento = default);

    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancelamento = default);

    Task<List<Usuario>> ListarAsync(CancellationToken cancelamento = default);

    Task<List<Usuario>> BuscarPorNomeAsync(string termo, CancellationToken cancelamento = default);

    Task<List<Usuario>> ListarComJogadoresAsync(CancellationToken cancelamento = default);

    Task<bool> ExisteEmailAsync(string email, CancellationToken cancelamento = default);
}
