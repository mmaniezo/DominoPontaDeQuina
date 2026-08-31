using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Domain.Interfaces;
using DominoPontaDeQuina.Service.Interfaces;

namespace DominoPontaDeQuina.Service.Services;

public class UsuarioService(IUsuarioRepository usuarios) : IUsuarioService
{
    public async Task<Usuario> CadastrarAsync(string nome, string email, string hashSenha, CancellationToken cancelamento = default)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do usuario e obrigatorio.", nameof(nome));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O email do usuario e obrigatorio.", nameof(email));
        if (string.IsNullOrWhiteSpace(hashSenha))
            throw new ArgumentException("A senha do usuario e obrigatoria.", nameof(hashSenha));

        if (await usuarios.ExisteEmailAsync(email, cancelamento))
            throw new InvalidOperationException($"Ja existe um usuario cadastrado com o email {email}.");

        var usuario = new Usuario
        {
            Nome = nome.Trim(),
            Email = email.Trim(),
            HashSenha = hashSenha
        };

        return await usuarios.AdicionarAsync(usuario, cancelamento);
    }

    public Task<List<Usuario>> ListarAsync(CancellationToken cancelamento = default) =>
        usuarios.ListarAsync(cancelamento);

    public Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancelamento = default) =>
        usuarios.ObterPorEmailAsync(email, cancelamento);

    public Task<List<Usuario>> BuscarPorNomeAsync(string termo, CancellationToken cancelamento = default) =>
        usuarios.BuscarPorNomeAsync(termo ?? string.Empty, cancelamento);
}
