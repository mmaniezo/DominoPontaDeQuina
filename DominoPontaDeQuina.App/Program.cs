using DominoPontaDeQuina.App;
using DominoPontaDeQuina.Domain.Interfaces;
using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Repository.Repositories;
using DominoPontaDeQuina.Service.Interfaces;
using DominoPontaDeQuina.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Ponto de composicao da aplicacao: aqui a montagem dos objetos e decidida.
var configuracao = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var stringConexao = configuracao.GetConnectionString("Domino")
    ?? throw new InvalidOperationException("A connection string 'Domino' nao foi configurada.");

var servicos = new ServiceCollection();

servicos.AddDbContext<DominoDbContext>(opcoes => opcoes.UseSqlite(stringConexao));

servicos.AddScoped<IUsuarioRepository, UsuarioRepository>();
servicos.AddScoped<IJogadorRepository, JogadorRepository>();
servicos.AddScoped<IPartidaRepository, PartidaRepository>();
servicos.AddScoped<IParticipacaoPartidaRepository, ParticipacaoPartidaRepository>();

servicos.AddScoped<IUsuarioService, UsuarioService>();
servicos.AddScoped<IJogadorService, JogadorService>();
servicos.AddScoped<IPartidaService, PartidaService>();

servicos.AddScoped<MenuPrincipal>();

using var provedor = servicos.BuildServiceProvider();
using var escopo = provedor.CreateScope();

var contexto = escopo.ServiceProvider.GetRequiredService<DominoDbContext>();
await contexto.Database.MigrateAsync();

var menu = escopo.ServiceProvider.GetRequiredService<MenuPrincipal>();
await menu.ExecutarAsync();
