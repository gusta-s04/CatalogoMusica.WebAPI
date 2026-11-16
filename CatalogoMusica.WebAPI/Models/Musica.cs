namespace CatalogoMusica.WebAPI.Models;

public class Musica
{
    public int IdMusica { get; set; }
    public int IdArtista { get; set; }
    public int? IdAlbum { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public int Duracao { get; set; }
    public string? URLAudio { get; set; }
    public string? CapaUrl { get; set; }
    public DateTime? DataLancamento { get; set; }
    public Artista Artista { get; set; } = null!;
    public Album? Album { get; set; }
    public ICollection<MusicaGenero> MusicasGeneros { get; set; } = new List<MusicaGenero>();
    public ICollection<PlaylistMusica> PlaylistsMusicas { get; set; } = new List<PlaylistMusica>();
    public ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
    public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
}