using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Service.Interfaces;

public interface IUsuarioService
{
    Task<Usuario> CadastrarAsync(string nome, string email, string hashSenha, CancellationToken cancelamento = default);

    Task<List<Usuario>> ListarAsync(CancellationToken cancelamento = default);

    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancelamento = default);

    Task<List<Usuario>> BuscarPorNomeAsync(string termo, CancellationToken cancelamento = default);
}
