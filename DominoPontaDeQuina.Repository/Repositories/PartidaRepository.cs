using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class PartidaRepository(DominoDbContext contexto)
{
    public async Task<Partida> AdicionarAsync(Partida partida, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(partida);
        await contexto.Partidas.AddAsync(partida, cancelamento);
        await contexto.SaveChangesAsync(cancelamento);
        return partida;
    }

    public async Task AtualizarAsync(Partida partida, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(partida);
        contexto.Partidas.Update(partida);
        await contexto.SaveChangesAsync(cancelamento);
    }

    public async Task RemoverAsync(Partida partida, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(partida);
        contexto.Partidas.Remove(partida);
        await contexto.SaveChangesAsync(cancelamento);
    }

    public Task<Partida?> ObterPorIdAsync(Guid id, CancellationToken cancelamento = default) =>
        contexto.Partidas
            .SingleOrDefaultAsync(partida => partida.Id == id, cancelamento);

    public Task<Partida?> ObterComParticipacoesAsync(Guid id, CancellationToken cancelamento = default) =>
        contexto.Partidas
            .AsNoTracking()
            .Include(partida => partida.Participacoes)
                .ThenInclude(participacao => participacao.Jogador)
            .SingleOrDefaultAsync(partida => partida.Id == id, cancelamento);

    public Task<List<Partida>> ListarPorStatusAsync(StatusJogo status, CancellationToken cancelamento = default) =>
        contexto.Partidas
            .AsNoTracking()
            .Where(partida => partida.Status == status)
            .OrderByDescending(partida => partida.IniciadoEm)
            .ToListAsync(cancelamento);

    public Task<List<Partida>> ListarEmAndamentoAsync(CancellationToken cancelamento = default) =>
        ListarPorStatusAsync(StatusJogo.EmAndamento, cancelamento);

    public Task<List<Partida>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancelamento = default) =>
        contexto.Partidas
            .AsNoTracking()
            .Where(partida => partida.IniciadoEm >= inicio && partida.IniciadoEm <= fim)
            .OrderBy(partida => partida.IniciadoEm)
            .ToListAsync(cancelamento);

    public Task<List<Partida>> ListarPorJogadorAsync(Guid jogadorId, CancellationToken cancelamento = default) =>
        contexto.Partidas
            .AsNoTracking()
            .Where(partida => partida.Participacoes.Any(participacao => participacao.JogadorId == jogadorId))
            .OrderByDescending(partida => partida.IniciadoEm)
            .ToListAsync(cancelamento);

    public Task<Partida?> ObterUltimaFinalizadaAsync(CancellationToken cancelamento = default) =>
        contexto.Partidas
            .AsNoTracking()
            .Where(partida => partida.Status == StatusJogo.Finalizado)
            .OrderByDescending(partida => partida.FinalizadoEm)
            .FirstOrDefaultAsync(cancelamento);

    public Task<int> ContarPorStatusAsync(StatusJogo status, CancellationToken cancelamento = default) =>
        contexto.Partidas
            .AsNoTracking()
            .CountAsync(partida => partida.Status == status, cancelamento);
}
