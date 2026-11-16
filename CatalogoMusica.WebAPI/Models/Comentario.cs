namespace CatalogoMusica.WebAPI.Models;

public class Comentario
{
    public int IdComentario { get; set; }
    public int IdMusica { get; set; }
    public int IdUsuario { get; set; }
    public string Mensagem { get; set; } = null!;
    public DateTime DataComentario { get; set; }
    public Musica Musica { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}