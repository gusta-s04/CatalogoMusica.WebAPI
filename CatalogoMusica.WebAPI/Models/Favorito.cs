namespace CatalogoMusica.WebAPI.Models;

public class Favorito
{
    public int IdUsuario { get; set; }
    public int IdMusica { get; set; }
    public DateTime DataFavorito { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public Musica Musica { get; set; } = null!;
}