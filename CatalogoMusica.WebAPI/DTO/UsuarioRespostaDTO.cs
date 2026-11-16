
using CatalogoMusica.WebAPI.Models;

namespace CatalogoMusica.WebAPI.DTO;

public record UsuarioRespostaDTO(
    int IdUsuario,
    string Nome,
    string Email,
    string TipoUsuario,
    bool Ativo
)
{
    public static UsuarioRespostaDTO Converter(Usuario u) =>
        new(
            u.IdUsuario,
            u.Nome,
            u.Email,
            u.TipoUsuario,
            u.Ativo
        );
}
