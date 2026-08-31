using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Domain.Interfaces;

namespace DominoPontaDeQuina.Tests.Fakes;

/// <summary>
/// Implementacao de teste do contrato de repositorio, sem banco de dados.
/// </summary>
public class FakeUsuarioRepository : IUsuarioRepository
{
    private readonly List<Usuario> _usuarios = [];

    public IReadOnlyList<Usuario> Usuarios => _usuarios;

    public Task<Usuario> AdicionarAsync(Usuario usuario, CancellationToken cancelamento = default)
    {
        _usuarios.Add(usuario);
        return Task.FromResult(usuario);
    }

    public Task AtualizarAsync(Usuario usuario, CancellationToken cancelamento = default) =>
        Task.CompletedTask;

    public Task RemoverAsync(Usuario usuario, CancellationToken cancelamento = default)
    {
        _usuarios.Remove(usuario);
        return Task.CompletedTask;
    }

    public Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancelamento = default) =>
        Task.FromResult(_usuarios.SingleOrDefault(usuario => usuario.Id == id));

    public Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancelamento = default) =>
        Task.FromResult(_usuarios.SingleOrDefault(usuario => usuario.Email == email));

    public Task<List<Usuario>> ListarAsync(CancellationToken cancelamento = default) =>
        Task.FromResult(_usuarios.OrderBy(usuario => usuario.Nome).ToList());

    public Task<List<Usuario>> BuscarPorNomeAsync(string termo, CancellationToken cancelamento = default) =>
        Task.FromResult(_usuarios.Where(usuario => usuario.Nome.Contains(termo)).ToList());

    public Task<List<Usuario>> ListarComJogadoresAsync(CancellationToken cancelamento = default) =>
        Task.FromResult(_usuarios.ToList());

    public Task<bool> ExisteEmailAsync(string email, CancellationToken cancelamento = default) =>
        Task.FromResult(_usuarios.Any(usuario => usuario.Email == email));
}
