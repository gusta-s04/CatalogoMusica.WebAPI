
using System.ComponentModel.DataAnnotations;

namespace CatalogoMusica.WebAPI.DTO;

public class UsuarioDTO
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(50)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Senha { get; set; } = string.Empty;
}
