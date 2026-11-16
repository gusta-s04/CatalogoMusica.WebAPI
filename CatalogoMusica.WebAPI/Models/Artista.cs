namespace CatalogoMusica.WebAPI.Models;

public class Artista
{
    public int IdArtista { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public ICollection<Album> Albuns { get; set; } = new List<Album>();
    public ICollection<Musica> Musicas { get; set; } = new List<Musica>();
}