
using CatalogoMusica.WebAPI.Context;
using CatalogoMusica.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CatalogoMusica.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FavoritosController(CatalogoMusicaDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var id = UsuarioId();

        return Ok(await db.Favoritos
            .AsNoTracking()
            .Where(x => x.IdUsuario == id)
            .Include(x => x.Musica)
            .ToListAsync());
    }

    [HttpPost("{idMusica:int}")]
    public async Task<IActionResult> Adicionar(int idMusica)
    {
        var id = UsuarioId();

        if (!await db.Musica.AnyAsync(x => x.IdMusica == idMusica))
            return NotFound("Música não encontrada.");

        if (await db.Favoritos.AnyAsync(x => x.IdUsuario == id && x.IdMusica == idMusica))
            return Conflict("Música já está nos favoritos.");

        db.Favoritos.Add(new Favorito
        {
            IdUsuario = id,
            IdMusica = idMusica
        });

        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{idMusica:int}")]
    public async Task<IActionResult> Remover(int idMusica)
    {
        var f = await db.Favoritos.FindAsync(UsuarioId(), idMusica);

        if (f == null)
            return NotFound();

        db.Favoritos.Remove(f);

        await db.SaveChangesAsync();

        return NoContent();
    }

    private int UsuarioId()
    {
        return int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
    }
}
