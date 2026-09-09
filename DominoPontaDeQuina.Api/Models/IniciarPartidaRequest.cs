namespace DominoPontaDeQuina.Api.Models;

/// <summary>Dados para iniciar uma nova partida.</summary>
/// <param name="PontuacaoAlvo">Pontuação necessária para vencer.</param>
public record IniciarPartidaRequest(int PontuacaoAlvo = 50);
