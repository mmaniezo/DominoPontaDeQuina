using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class UsuarioRepository(DominoDbContext contexto)
{
    public async Task<Usuario> AdicionarAsync(Usuario usuario, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        await contexto.Usuarios.AddAsync(usuario, cancelamento);
        await contexto.SaveChangesAsync(cancelamento);
        return usuario;
    }

    public async Task AtualizarAsync(Usuario usuario, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        contexto.Usuarios.Update(usuario);
        await contexto.SaveChangesAsync(cancelamento);
    }

    public async Task RemoverAsync(Usuario usuario, CancellationToken cancelamento = default)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        contexto.Usuarios.Remove(usuario);
        await contexto.SaveChangesAsync(cancelamento);
    }

    public Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancelamento = default) =>
        contexto.Usuarios
            .SingleOrDefaultAsync(usuario => usuario.Id == id, cancelamento);

    public Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancelamento = default) =>
        contexto.Usuarios
            .SingleOrDefaultAsync(usuario => usuario.Email == email, cancelamento);

    public Task<List<Usuario>> ListarAsync(CancellationToken cancelamento = default) =>
        contexto.Usuarios
            .AsNoTracking()
            .OrderBy(usuario => usuario.Nome)
            .ToListAsync(cancelamento);

    public Task<List<Usuario>> BuscarPorNomeAsync(string termo, CancellationToken cancelamento = default) =>
        contexto.Usuarios
            .AsNoTracking()
            .Where(usuario => usuario.Nome.Contains(termo))
            .OrderBy(usuario => usuario.Nome)
            .ToListAsync(cancelamento);

    public Task<List<Usuario>> ListarComJogadoresAsync(CancellationToken cancelamento = default) =>
        contexto.Usuarios
            .AsNoTracking()
            .Include(usuario => usuario.Jogadores)
            .OrderBy(usuario => usuario.Nome)
            .ToListAsync(cancelamento);

    public Task<bool> ExisteEmailAsync(string email, CancellationToken cancelamento = default) =>
        contexto.Usuarios
            .AsNoTracking()
            .AnyAsync(usuario => usuario.Email == email, cancelamento);
}
