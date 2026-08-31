using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Repository.Repositories;
using DominoPontaDeQuina.Service.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Tests.Services;

/// <summary>
/// Exercita o fluxo principal com os repositories reais, mantendo as consultas LINQ.
/// </summary>
public class PartidaServiceTests : IDisposable
{
    private readonly SqliteConnection _conexao;
    private readonly DominoDbContext _contexto;
    private readonly PartidaService _servico;

    public PartidaServiceTests()
    {
        _conexao = new SqliteConnection("Filename=:memory:");
        _conexao.Open();

        var opcoes = new DbContextOptionsBuilder<DominoDbContext>()
            .UseSqlite(_conexao)
            .Options;

        _contexto = new DominoDbContext(opcoes);
        _contexto.Database.EnsureCreated();

        _servico = new PartidaService(
            new PartidaRepository(_contexto),
            new ParticipacaoPartidaRepository(_contexto),
            new JogadorRepository(_contexto));
    }

    [Fact(DisplayName = "Deve iniciar partida com os jogadores informados")]
    public async Task DeveIniciarPartida()
    {
        var (primeiro, segundo) = await CriarJogadoresAsync();

        var partida = await _servico.IniciarAsync([primeiro, segundo]);
        var salva = await _servico.ObterComParticipacoesAsync(partida.Id);

        Assert.NotNull(salva);
        Assert.Equal(StatusJogo.EmAndamento, salva.Status);
        Assert.Equal(2, salva.Participacoes.Count);
        Assert.Equal([1, 2], salva.Participacoes.Select(participacao => participacao.Posicao).Order());
    }

    [Fact(DisplayName = "Nao deve iniciar partida com menos de dois jogadores")]
    public async Task NaoDeveIniciarPartidaSozinho()
    {
        var (primeiro, _) = await CriarJogadoresAsync();

        await Assert.ThrowsAsync<ArgumentException>(() => _servico.IniciarAsync([primeiro]));
    }

    [Fact(DisplayName = "Nao deve iniciar partida com jogador inexistente")]
    public async Task NaoDeveIniciarPartidaComJogadorInexistente()
    {
        var (primeiro, _) = await CriarJogadoresAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _servico.IniciarAsync([primeiro, Guid.NewGuid()]));
    }

    [Fact(DisplayName = "Deve registrar pontuacao e finalizar a partida com vencedor")]
    public async Task DeveFinalizarPartida()
    {
        var (primeiro, segundo) = await CriarJogadoresAsync();
        var partida = await _servico.IniciarAsync([primeiro, segundo]);

        await _servico.RegistrarPontuacaoAsync(partida.Id, primeiro, 100);
        await _servico.RegistrarPontuacaoAsync(partida.Id, segundo, 60);
        await _servico.FinalizarAsync(partida.Id, primeiro);

        var finalizada = await _servico.ObterComParticipacoesAsync(partida.Id);

        Assert.NotNull(finalizada);
        Assert.Equal(StatusJogo.Finalizado, finalizada.Status);
        Assert.NotNull(finalizada.FinalizadoEm);

        var vencedor = Assert.Single(finalizada.Participacoes, participacao => participacao.Vencedor);
        Assert.Equal(primeiro, vencedor.JogadorId);
        Assert.Equal(100, vencedor.Pontuacao);
    }

    [Fact(DisplayName = "Nao deve finalizar partida que ja foi finalizada")]
    public async Task NaoDeveFinalizarDuasVezes()
    {
        var (primeiro, segundo) = await CriarJogadoresAsync();
        var partida = await _servico.IniciarAsync([primeiro, segundo]);

        await _servico.FinalizarAsync(partida.Id, primeiro);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _servico.FinalizarAsync(partida.Id, segundo));
    }

    [Fact(DisplayName = "Deve listar partidas em andamento e partidas do jogador")]
    public async Task DeveListarPartidas()
    {
        var (primeiro, segundo) = await CriarJogadoresAsync();

        var emAndamento = await _servico.IniciarAsync([primeiro, segundo]);
        var finalizada = await _servico.IniciarAsync([primeiro, segundo]);
        await _servico.FinalizarAsync(finalizada.Id, segundo);

        var abertas = await _servico.ListarEmAndamentoAsync();
        var doJogador = await _servico.ListarPorJogadorAsync(primeiro);

        Assert.Equal(emAndamento.Id, Assert.Single(abertas).Id);
        Assert.Equal(2, doJogador.Count);
    }

    [Fact(DisplayName = "Deve montar ranking pelas maiores pontuacoes")]
    public async Task DeveMontarRanking()
    {
        var (primeiro, segundo) = await CriarJogadoresAsync();
        var partida = await _servico.IniciarAsync([primeiro, segundo]);

        await _servico.RegistrarPontuacaoAsync(partida.Id, primeiro, 40);
        await _servico.RegistrarPontuacaoAsync(partida.Id, segundo, 90);

        var ranking = await _servico.ObterRankingAsync(2);

        Assert.Equal(2, ranking.Count);
        Assert.Equal(segundo, ranking[0].JogadorId);
        Assert.Equal(90, ranking[0].Pontuacao);
    }

    private async Task<(Guid Primeiro, Guid Segundo)> CriarJogadoresAsync()
    {
        var usuario = new Usuario
        {
            Nome = "Usuario Teste",
            Email = $"{Guid.NewGuid()}@teste.com",
            HashSenha = "hash"
        };

        var primeiro = new Jogador { UsuarioId = usuario.Id, NomeExibicao = "Jogador 1" };
        var segundo = new Jogador { UsuarioId = usuario.Id, NomeExibicao = "Jogador 2" };

        _contexto.Usuarios.Add(usuario);
        _contexto.Jogadores.AddRange(primeiro, segundo);
        await _contexto.SaveChangesAsync();

        return (primeiro.Id, segundo.Id);
    }

    public void Dispose()
    {
        _contexto.Dispose();
        _conexao.Dispose();
        GC.SuppressFinalize(this);
    }
}
