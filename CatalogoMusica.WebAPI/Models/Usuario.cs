namespace CatalogoMusica.WebAPI.Models;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string TipoUsuario { get; set; } = "Usuario";
    public bool Ativo { get; set; } = true;
    public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    public ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
    public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
}