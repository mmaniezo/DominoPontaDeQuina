namespace DominoPontaDeQuina.Api.Models;

/// <summary>Dados para registrar um lance em uma partida.</summary>
/// <param name="JogadorId">Identificador do jogador que realizou o lance.</param>
public record RegistrarLanceRequest(Guid JogadorId);
