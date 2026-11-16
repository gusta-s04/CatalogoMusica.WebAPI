namespace CatalogoMusica.WebAPI.Models;

public class MusicaGenero
{
    public int IdMusica { get; set; }
    public int IdGenero { get; set; }
    public Musica Musica { get; set; } = null!;
    public Genero Genero { get; set; } = null!;
}