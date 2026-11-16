
namespace CatalogoMusica.WebAPI.Interfaces;

public interface IRepository<T> where T : class
{
    Task<List<T>> Listar();

    Task<T?> BuscarPorId(int id);

    Task<T> Cadastrar(T entidade);

    Task Atualizar(T entidade);

    Task Excluir(T entidade);
}
