namespace CatalogoMusica.WebAPI.Models;

public class Album
{
    public int IdAlbum { get; set; }
    public int IdArtista { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public string? CapaUrl { get; set; }
    public DateTime? DataLancamento { get; set; }
    public Artista Artista { get; set; } = null!;
    public ICollection<Musica> Musicas { get; set; } = new List<Musica>();
}