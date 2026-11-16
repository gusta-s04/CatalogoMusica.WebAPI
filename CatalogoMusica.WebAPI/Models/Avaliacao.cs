namespace CatalogoMusica.WebAPI.Models;

public class Avaliacao
{
    public int IdAvaliacao { get; set; }
    public int IdMusica { get; set; }
    public int IdUsuario { get; set; }
    public int Nota { get; set; }
    public DateTime DataAvaliacao { get; set; }
    public Musica Musica { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}