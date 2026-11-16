
using CatalogoMusica.WebAPI.Context;
using CatalogoMusica.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMusica.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusicaGeneroController(CatalogoMusicaDbContext db) : ControllerBase
{
    [HttpGet("{idMusica:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Listar(int idMusica)
    {
        return Ok(await db.MusicaGenero
            .AsNoTracking()
            .Where(x => x.IdMusica == idMusica)
            .Include(x => x.Genero)
            .ToListAsync());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Adicionar(MusicaGenero item)
    {
        db.MusicaGenero.Add(item);

        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{idMusica:int}/{idGenero:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Remover(int idMusica, int idGenero)
    {
        var item = await db.MusicaGenero.FindAsync(idMusica, idGenero);

        if (item == null)
            return NotFound();

        db.MusicaGenero.Remove(item);

        await db.SaveChangesAsync();

        return NoContent();
    }
}
