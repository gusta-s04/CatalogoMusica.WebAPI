namespace CatalogoMusica.WebAPI.Models;

public class Genero
{
    public int IdGenero { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public ICollection<MusicaGenero> MusicasGeneros { get; set; } = new List<MusicaGenero>();
}