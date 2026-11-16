
using CatalogoMusica.WebAPI.DTO;
using CatalogoMusica.WebAPI.Interfaces;
using CatalogoMusica.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CatalogoMusica.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController(IUsuarioRepository repo) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Listar()
    {
        return Ok((await repo.Listar())
            .Select(UsuarioRespostaDTO.Converter));
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Buscar(int id)
    {
        if (!PodeAcessar(id))
            return Forbid();

        var u = await repo.BuscarPorId(id);

        return u == null
            ? NotFound()
            : Ok(UsuarioRespostaDTO.Converter(u));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Cadastrar(UsuarioDTO dto)
    {
        var u = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha
        };

        try
        {
            await repo.Cadastrar(u);
        }
        catch (DbUpdateException)
        {
            return Conflict("Este e-mail já está cadastrado.");
        }

        return CreatedAtAction(
            nameof(Buscar),
            new { id = u.IdUsuario },
            UsuarioRespostaDTO.Converter(u)
        );
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Atualizar(int id, UsuarioDTO dto)
    {
        if (!PodeAcessar(id))
            return Forbid();

        var atual = await repo.BuscarPorId(id);

        if (atual == null)
            return NotFound();

        try
        {
            await repo.Atualizar(id, new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha
            });
        }
        catch (DbUpdateException)
        {
            return Conflict("Este e-mail já está cadastrado.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Excluir(int id)
    {
        if (!PodeAcessar(id))
            return Forbid();

        if (await repo.BuscarPorId(id) == null)
            return NotFound();

        await repo.Excluir(id);

        return NoContent();
    }

    private bool PodeAcessar(int id)
    {
        return User.IsInRole("Admin") ||
               (int.TryParse(
                   User.FindFirstValue(JwtRegisteredClaimNames.Sub),
                   out var uid
               ) && uid == id);
    }
}
