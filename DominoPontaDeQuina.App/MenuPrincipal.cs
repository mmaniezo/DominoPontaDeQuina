using DominoPontaDeQuina.Service.Interfaces;

namespace DominoPontaDeQuina.App;

/// <summary>
/// Classe de entrada do fluxo principal. Recebe as dependencias por construtor.
/// </summary>
public class MenuPrincipal(
    IUsuarioService usuarios,
    IJogadorService jogadores,
    IPartidaService partidas)
{
    public async Task ExecutarAsync(CancellationToken cancelamento = default)
    {
        var sair = false;

        while (!sair)
        {
            ExibirOpcoes();

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    await CadastrarUsuarioAsync(cancelamento);
                    break;
                case "2":
                    await ListarUsuariosAsync(cancelamento);
                    break;
                case "3":
                    await CadastrarJogadorAsync(cancelamento);
                    break;
                case "4":
                    await ListarJogadoresAsync(cancelamento);
                    break;
                case "5":
                    await IniciarPartidaAsync(cancelamento);
                    break;
                case "6":
                    await RegistrarPontuacaoAsync(cancelamento);
                    break;
                case "7":
                    await FinalizarPartidaAsync(cancelamento);
                    break;
                case "8":
                    await ListarPartidasEmAndamentoAsync(cancelamento);
                    break;
                case "9":
                    await ExibirRankingAsync(cancelamento);
                    break;
                case "0":
                    sair = true;
                    break;
                default:
                    Console.WriteLine("Opcao invalida.");
                    break;
            }
        }
    }

    private static void ExibirOpcoes()
    {
        Console.WriteLine();
        Console.WriteLine("=== Domino Ponta de Quina ===");
        Console.WriteLine("1 - Cadastrar usuario");
        Console.WriteLine("2 - Listar usuarios");
        Console.WriteLine("3 - Cadastrar jogador");
        Console.WriteLine("4 - Listar jogadores de um usuario");
        Console.WriteLine("5 - Iniciar partida");
        Console.WriteLine("6 - Registrar pontuacao");
        Console.WriteLine("7 - Finalizar partida");
        Console.WriteLine("8 - Listar partidas em andamento");
        Console.WriteLine("9 - Ranking de pontuacoes");
        Console.WriteLine("0 - Sair");
        Console.Write("Opcao: ");
    }

    private async Task CadastrarUsuarioAsync(CancellationToken cancelamento)
    {
        var nome = LerTexto("Nome");
        var email = LerTexto("Email");
        var senha = LerTexto("Senha");

        await ExecutarComTratamentoAsync(async () =>
        {
            var usuario = await usuarios.CadastrarAsync(nome, email, senha, cancelamento);
            Console.WriteLine($"Usuario cadastrado: {usuario.Id}");
        });
    }

    private async Task ListarUsuariosAsync(CancellationToken cancelamento)
    {
        var lista = await usuarios.ListarAsync(cancelamento);

        if (lista.Count == 0)
        {
            Console.WriteLine("Nenhum usuario cadastrado.");
            return;
        }

        foreach (var usuario in lista)
            Console.WriteLine($"{usuario.Id} - {usuario.Nome} ({usuario.Email})");
    }

    private async Task CadastrarJogadorAsync(CancellationToken cancelamento)
    {
        var usuarioId = LerGuid("Id do usuario");
        var nomeExibicao = LerTexto("Nome de exibicao");

        await ExecutarComTratamentoAsync(async () =>
        {
            var jogador = await jogadores.CadastrarAsync(usuarioId, nomeExibicao, cancelamento);
            Console.WriteLine($"Jogador cadastrado: {jogador.Id}");
        });
    }

    private async Task ListarJogadoresAsync(CancellationToken cancelamento)
    {
        var usuarioId = LerGuid("Id do usuario");
        var lista = await jogadores.ListarPorUsuarioAsync(usuarioId, cancelamento);

        if (lista.Count == 0)
        {
            Console.WriteLine("Nenhum jogador encontrado para o usuario informado.");
            return;
        }

        foreach (var jogador in lista)
        {
            var vitorias = await jogadores.ContarVitoriasAsync(jogador.Id, cancelamento);
            var pontuacao = await jogadores.ObterPontuacaoTotalAsync(jogador.Id, cancelamento);
            Console.WriteLine($"{jogador.Id} - {jogador.NomeExibicao} | vitorias: {vitorias} | pontos: {pontuacao}");
        }
    }

    private async Task IniciarPartidaAsync(CancellationToken cancelamento)
    {
        Console.Write("Ids dos jogadores separados por ponto e virgula: ");
        var entrada = Console.ReadLine() ?? string.Empty;

        var ids = new List<Guid>();

        foreach (var parte in entrada.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (!Guid.TryParse(parte, out var id))
            {
                Console.WriteLine($"Id invalido: {parte}");
                return;
            }

            ids.Add(id);
        }

        await ExecutarComTratamentoAsync(async () =>
        {
            var partida = await partidas.IniciarAsync(ids, cancelamento);
            Console.WriteLine($"Partida iniciada: {partida.Id}");
        });
    }

    private async Task RegistrarPontuacaoAsync(CancellationToken cancelamento)
    {
        var partidaId = LerGuid("Id da partida");
        var jogadorId = LerGuid("Id do jogador");

        Console.Write("Pontuacao: ");

        if (!int.TryParse(Console.ReadLine(), out var pontuacao))
        {
            Console.WriteLine("Pontuacao invalida.");
            return;
        }

        await ExecutarComTratamentoAsync(async () =>
        {
            await partidas.RegistrarPontuacaoAsync(partidaId, jogadorId, pontuacao, cancelamento);
            Console.WriteLine("Pontuacao registrada.");
        });
    }

    private async Task FinalizarPartidaAsync(CancellationToken cancelamento)
    {
        var partidaId = LerGuid("Id da partida");
        var vencedorId = LerGuid("Id do jogador vencedor");

        await ExecutarComTratamentoAsync(async () =>
        {
            var partida = await partidas.FinalizarAsync(partidaId, vencedorId, cancelamento);
            Console.WriteLine($"Partida finalizada em {partida.FinalizadoEm:u}.");
        });
    }

    private async Task ListarPartidasEmAndamentoAsync(CancellationToken cancelamento)
    {
        var lista = await partidas.ListarEmAndamentoAsync(cancelamento);

        if (lista.Count == 0)
        {
            Console.WriteLine("Nenhuma partida em andamento.");
            return;
        }

        foreach (var partida in lista)
            Console.WriteLine($"{partida.Id} - iniciada em {partida.IniciadoEm:u}");
    }

    private async Task ExibirRankingAsync(CancellationToken cancelamento)
    {
        var ranking = await partidas.ObterRankingAsync(10, cancelamento);

        if (ranking.Count == 0)
        {
            Console.WriteLine("Nenhuma participacao registrada.");
            return;
        }

        foreach (var participacao in ranking)
            Console.WriteLine($"{participacao.Jogador.NomeExibicao} - {participacao.Pontuacao} pontos");
    }

    private static string LerTexto(string rotulo)
    {
        Console.Write($"{rotulo}: ");
        return Console.ReadLine() ?? string.Empty;
    }

    private static Guid LerGuid(string rotulo)
    {
        Console.Write($"{rotulo}: ");
        return Guid.TryParse(Console.ReadLine(), out var id) ? id : Guid.Empty;
    }

    private static async Task ExecutarComTratamentoAsync(Func<Task> acao)
    {
        try
        {
            await acao();
        }
        catch (Exception excecao) when (excecao is ArgumentException or InvalidOperationException)
        {
            Console.WriteLine($"Erro: {excecao.Message}");
        }
    }
}
