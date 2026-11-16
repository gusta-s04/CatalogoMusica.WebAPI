
using CatalogoMusica.WebAPI.DTO;
using CatalogoMusica.WebAPI.Interfaces;
using CatalogoMusica.WebAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CatalogoMusica.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController(IUsuarioRepository repo, JwtSettings jwt) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Entrar(LoginDTO dto)
    {
        var u = await repo.BuscarPorEmailESenha(dto.Email, dto.Senha);

        if (u == null)
            return Unauthorized(new
            {
                mensagem = "E-mail ou senha inválidos."
            });

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, u.IdUsuario.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, u.Email),
            new Claim(ClaimTypes.Role, u.TipoUsuario),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var exp = DateTime.UtcNow.AddMinutes(jwt.ExpirationMinutes);

        var cred = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            jwt.Issuer,
            jwt.Audience,
            claims,
            expires: exp,
            signingCredentials: cred
        );

        return Ok(new TokenDTO(
            new JwtSecurityTokenHandler().WriteToken(token),
            u.IdUsuario,
            u.Nome,
            u.Email,
            u.TipoUsuario,
            exp
        ));
    }
}
