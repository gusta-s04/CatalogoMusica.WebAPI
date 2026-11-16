
using CatalogoMusica.WebAPI.Context;
using CatalogoMusica.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMusica.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComentarioController(CatalogoMusicaDbContext context) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Listar()
    {
        return Ok(await context.Comentario
            .AsNoTracking()
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Buscar(int id)
    {
        var item = await context.Comentario.FindAsync(id);

        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Cadastrar(Comentario item)
    {
        context.Comentario.Add(item);

        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Buscar),
            new { id = item.IdComentario },
            item
        );
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Atualizar(int id, Comentario item)
    {
        if (id != item.IdComentario)
            return BadRequest();

        if (!await context.Comentario.AnyAsync(x => x.IdComentario == id))
            return NotFound();

        context.Entry(item).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Excluir(int id)
    {
        var item = await context.Comentario.FindAsync(id);

        if (item == null)
            return NotFound();

        context.Comentario.Remove(item);

        await context.SaveChangesAsync();

        return NoContent();
    }
}
