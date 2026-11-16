
namespace CatalogoMusica.WebAPI.DTO;

public record TokenDTO(
    string Token,
    int IdUsuario,
    string Nome,
    string Email,
    string TipoUsuario,
    DateTime Expiracao
);
