namespace CatalogoMusica.WebAPI.Models;

public class PlaylistMusica
{
    public int IdPlaylist { get; set; }
    public int IdMusica { get; set; }
    public DateTime DataAdicao { get; set; }
    public Playlist Playlist { get; set; } = null!;
    public Musica Musica { get; set; } = null!;
}