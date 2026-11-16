using System.Security.Cryptography;
using System.Text;
using CatalogoMusica.WebAPI.Context;
using CatalogoMusica.WebAPI.Interfaces;
using CatalogoMusica.WebAPI.Models;
using CatalogoMusica.WebAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMusica.WebAPI.Repositories;

public class UsuarioRepository(CatalogoMusicaDbContext context) : IUsuarioRepository
{
    public async Task<Usuario?> BuscarPorEmailESenha(string email, string senha)
    {
        var usuario = await context.Usuario
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email && x.Ativo);

        return usuario != null && Criptografia.CompararHash(senha, usuario.Senha)
            ? usuario
            : null;
    }

    public Task<Usuario?> BuscarPorId(int id) =>
        context.Usuario
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdUsuario == id);

    public Task<List<Usuario>> Listar() =>
        context.Usuario
            .AsNoTracking()
            .Where(x => x.Ativo)
            .ToListAsync();

    public async Task Cadastrar(Usuario usuario)
    {
        usuario.Senha = Criptografia.GerarHash(usuario.Senha);
        usuario.TipoUsuario = "Usuario";
        usuario.Ativo = true;

        context.Usuario.Add(usuario);
        await context.SaveChangesAsync();
    }

    public async Task Atualizar(int id, Usuario dados)
    {
        var usuario = await context.Usuario.FindAsync(id);
        if (usuario == null)
            return;

        usuario.Nome = dados.Nome;
        usuario.Email = dados.Email;

        if (!string.IsNullOrWhiteSpace(dados.Senha))
            usuario.Senha = Criptografia.GerarHash(dados.Senha);

        await context.SaveChangesAsync();
    }

    public async Task Excluir(int id)
    {
        var usuario = await context.Usuario.FindAsync(id);
        if (usuario != null)
        {
            context.Usuario.Remove(usuario);
            await context.SaveChangesAsync();
        }
    }

    public async Task GarantirAdministradorInicial()
    {
        var admin = await context.Usuario
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Email == "admin@catalogomusicas.com");

        if (admin == null)
        {
            context.Usuario.Add(new Usuario
            {
                Nome = "Administrador",
                Email = "admin@catalogomusicas.com",
                Senha = Criptografia.GerarHash("admin123"),
                TipoUsuario = "Admin",
                Ativo = true
            });

            await context.SaveChangesAsync();
            return;
        }

        if (!admin.Senha.StartsWith("$2", StringComparison.Ordinal))
        {
            var legado = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes("admin123")));

            if (string.Equals(admin.Senha, legado, StringComparison.OrdinalIgnoreCase))
            {
                admin.Senha = Criptografia.GerarHash("admin123");
                await context.SaveChangesAsync();
            }
        }
    }
}