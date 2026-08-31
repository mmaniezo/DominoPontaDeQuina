using DominoPontaDeQuina.Service.Services;
using DominoPontaDeQuina.Tests.Fakes;

namespace DominoPontaDeQuina.Tests.Services;

public class UsuarioServiceTests
{
    [Fact(DisplayName = "Deve cadastrar usuario usando o contrato de repositorio")]
    public async Task DeveCadastrarUsuario()
    {
        var repositorio = new FakeUsuarioRepository();
        var servico = new UsuarioService(repositorio);

        var usuario = await servico.CadastrarAsync("Rafael", "rafael@teste.com", "hash");

        Assert.Single(repositorio.Usuarios);
        Assert.Equal("Rafael", usuario.Nome);
        Assert.Equal("rafael@teste.com", usuario.Email);
    }

    [Fact(DisplayName = "Nao deve cadastrar usuario com email repetido")]
    public async Task NaoDeveCadastrarEmailRepetido()
    {
        var repositorio = new FakeUsuarioRepository();
        var servico = new UsuarioService(repositorio);

        await servico.CadastrarAsync("Rafael", "rafael@teste.com", "hash");

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => servico.CadastrarAsync("Outro", "rafael@teste.com", "hash"));
    }

    [Fact(DisplayName = "Nao deve cadastrar usuario sem nome")]
    public async Task NaoDeveCadastrarSemNome()
    {
        var servico = new UsuarioService(new FakeUsuarioRepository());

        await Assert.ThrowsAsync<ArgumentException>(
            () => servico.CadastrarAsync(" ", "rafael@teste.com", "hash"));
    }
}
