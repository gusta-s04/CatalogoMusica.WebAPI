namespace CatalogoMusica.WebAPI.Models;

public class Playlist
{
    public int IdPlaylist { get; set; }
    public int IdUsuario { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public bool Privada { get; set; } = true;
    public DateTime DataCriacao { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public ICollection<PlaylistMusica> Musicas { get; set; } = new List<PlaylistMusica>();
}