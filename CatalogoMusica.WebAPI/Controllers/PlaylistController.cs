
using CatalogoMusica.WebAPI.Context;
using CatalogoMusica.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMusica.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistController(CatalogoMusicaDbContext context) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Listar()
    {
        return Ok(await context.Playlist
            .AsNoTracking()
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Buscar(int id)
    {
        var item = await context.Playlist.FindAsync(id);

        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Cadastrar(Playlist item)
    {
        context.Playlist.Add(item);

        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Buscar),
            new { id = item.IdPlaylist },
            item
        );
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Atualizar(int id, Playlist item)
    {
        if (id != item.IdPlaylist)
            return BadRequest();

        if (!await context.Playlist.AnyAsync(x => x.IdPlaylist == id))
            return NotFound();

        context.Entry(item).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Excluir(int id)
    {
        var item = await context.Playlist.FindAsync(id);

        if (item == null)
            return NotFound();

        context.Playlist.Remove(item);

        await context.SaveChangesAsync();

        return NoContent();
    }
}
