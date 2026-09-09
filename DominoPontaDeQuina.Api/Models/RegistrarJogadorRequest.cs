namespace DominoPontaDeQuina.Api.Models;

/// <summary>Dados para registrar um jogador em uma partida.</summary>
/// <param name="Nome">Nome do jogador.</param>
/// <param name="UsuarioId">Conta opcional associada ao jogador.</param>
public record RegistrarJogadorRequest(string Nome, Guid? UsuarioId = null);
