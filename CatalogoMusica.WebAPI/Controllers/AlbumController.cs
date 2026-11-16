
using CatalogoMusica.WebAPI.Context;
using CatalogoMusica.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMusica.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumController(CatalogoMusicaDbContext context) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Listar()
    {
        return Ok(await context.Album
            .AsNoTracking()
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Buscar(int id)
    {
        var item = await context.Album.FindAsync(id);

        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cadastrar(Album item)
    {
        context.Album.Add(item);

        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Buscar),
            new { id = item.IdAlbum },
            item
        );
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Atualizar(int id, Album item)
    {
        if (id != item.IdAlbum)
            return BadRequest();

        if (!await context.Album.AnyAsync(x => x.IdAlbum == id))
            return NotFound();

        context.Entry(item).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Excluir(int id)
    {
        var item = await context.Album.FindAsync(id);

        if (item == null)
            return NotFound();

        context.Album.Remove(item);

        await context.SaveChangesAsync();

        return NoContent();
    }
}
