
using CatalogoMusica.WebAPI.Context;
using CatalogoMusica.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMusica.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusicaController(CatalogoMusicaDbContext context) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Listar()
    {
        return Ok(await context.Musica
            .AsNoTracking()
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Buscar(int id)
    {
        var item = await context.Musica.FindAsync(id);

        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cadastrar(Musica item)
    {
        context.Musica.Add(item);

        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Buscar),
            new { id = item.IdMusica },
            item
        );
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Atualizar(int id, Musica item)
    {
        if (id != item.IdMusica)
            return BadRequest();

        if (!await context.Musica.AnyAsync(x => x.IdMusica == id))
            return NotFound();

        context.Entry(item).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Excluir(int id)
    {
        var item = await context.Musica.FindAsync(id);

        if (item == null)
            return NotFound();

        context.Musica.Remove(item);

        await context.SaveChangesAsync();

        return NoContent();
    }
}
