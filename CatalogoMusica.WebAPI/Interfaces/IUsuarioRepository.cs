using CatalogoMusica.WebAPI.Models;

namespace CatalogoMusica.WebAPI.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> BuscarPorEmailESenha(string email, string senha);
    Task<Usuario?> BuscarPorId(int id);
    Task<List<Usuario>> Listar();
    Task Cadastrar(Usuario usuario);
    Task Atualizar(int id, Usuario usuario);
    Task Excluir(int id);
    Task GarantirAdministradorInicial();
}