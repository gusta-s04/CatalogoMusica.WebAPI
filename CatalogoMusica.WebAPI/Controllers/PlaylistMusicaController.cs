
using CatalogoMusica.WebAPI.Context;
using CatalogoMusica.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMusica.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlaylistMusicaController(CatalogoMusicaDbContext db) : ControllerBase
{
    [HttpGet("{idPlaylist:int}")]
    public async Task<IActionResult> Listar(int idPlaylist)
    {
        return Ok(await db.PlaylistMusica
            .AsNoTracking()
            .Where(x => x.IdPlaylist == idPlaylist)
            .Include(x => x.Musica)
            .ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Adicionar(PlaylistMusica item)
    {
        if (!await db.Playlist.AnyAsync(x => x.IdPlaylist == item.IdPlaylist))
            return NotFound("Playlist não encontrada.");

        if (!await db.Musica.AnyAsync(x => x.IdMusica == item.IdMusica))
            return NotFound("Música não encontrada.");

        db.PlaylistMusica.Add(item);

        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{idPlaylist:int}/{idMusica:int}")]
    public async Task<IActionResult> Remover(int idPlaylist, int idMusica)
    {
        var item = await db.PlaylistMusica.FindAsync(idPlaylist, idMusica);

        if (item == null)
            return NotFound();

        db.PlaylistMusica.Remove(item);

        await db.SaveChangesAsync();

        return NoContent();
    }
}
