using CatalogoMusica.WebAPI.Context;
using CatalogoMusica.WebAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMusica.WebAPI.Repositories;

public class Repository<T>(CatalogoMusicaDbContext context) : IRepository<T> where T : class
{
    public Task<List<T>> Listar() =>
        context.Set<T>().AsNoTracking().ToListAsync();

    public async Task<T?> BuscarPorId(int id) =>
        await context.Set<T>().FindAsync(id);

    public async Task<T> Cadastrar(T entidade)
    {
        context.Set<T>().Add(entidade);
        await context.SaveChangesAsync();
        return entidade;
    }

    public async Task Atualizar(T entidade)
    {
        context.Entry(entidade).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task Excluir(T entidade)
    {
        context.Set<T>().Remove(entidade);
        await context.SaveChangesAsync();
    }
}