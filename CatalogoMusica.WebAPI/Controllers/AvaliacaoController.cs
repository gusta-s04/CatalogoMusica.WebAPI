
using CatalogoMusica.WebAPI.Context;
using CatalogoMusica.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMusica.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvaliacaoController(CatalogoMusicaDbContext context) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Listar()
    {
        return Ok(await context.Avaliacao
            .AsNoTracking()
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Buscar(int id)
    {
        var item = await context.Avaliacao.FindAsync(id);

        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Cadastrar(Avaliacao item)
    {
        context.Avaliacao.Add(item);

        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Buscar),
            new { id = item.IdAvaliacao },
            item
        );
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Atualizar(int id, Avaliacao item)
    {
        if (id != item.IdAvaliacao)
            return BadRequest();

        if (!await context.Avaliacao.AnyAsync(x => x.IdAvaliacao == id))
            return NotFound();

        context.Entry(item).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Excluir(int id)
    {
        var item = await context.Avaliacao.FindAsync(id);

        if (item == null)
            return NotFound();

        context.Avaliacao.Remove(item);

        await context.SaveChangesAsync();

        return NoContent();
    }
}
