using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

using CatalogoMusica.WebAPI.Context;
using CatalogoMusica.WebAPI.Interfaces;
using CatalogoMusica.WebAPI.Repositories;
using CatalogoMusica.WebAPI.Utils;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatalogoMusicaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Configure DefaultConnection.")
    )
);

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
    );

var jwt = builder.Configuration
    .GetSection(JwtSettings.SectionName)
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException("Configure Jwt.");

if (string.IsNullOrWhiteSpace(jwt.Issuer) ||
    string.IsNullOrWhiteSpace(jwt.Audience) ||
    string.IsNullOrWhiteSpace(jwt.Key) ||
    Encoding.UTF8.GetByteCount(jwt.Key) < 32 ||
    jwt.ExpirationMinutes is < 1 or > 1440)
{
    throw new InvalidOperationException(
        "Configure Jwt:Key (mínimo 32 bytes), Issuer, Audience e ExpirationMinutes."
    );
}

builder.Services.AddSingleton(jwt);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,

            ValidateAudience = true,
            ValidAudience = jwt.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt.Key)
            ),

            RequireSignedTokens = true,
            RequireExpirationTime = true,

            ClockSkew = TimeSpan.Zero,

            NameClaimType = JwtRegisteredClaimNames.Sub,
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var idTexto = context.Principal?
                    .FindFirstValue(JwtRegisteredClaimNames.Sub);

                if (!int.TryParse(idTexto, out var id))
                {
                    context.Fail("Token sem ID de usuário.");
                    return;
                }

                var repo = context.HttpContext.RequestServices
                    .GetRequiredService<IUsuarioRepository>();

                var usuario = await repo.BuscarPorId(id);

                if (usuario == null ||
                    !usuario.Ativo ||
                    context.Principal?.FindFirstValue(ClaimTypes.Role) != usuario.TipoUsuario)
                {
                    context.Fail(
                        "Usuário inativo, inexistente ou com permissão alterada."
                    );
                }
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CatalogoMusica.WebAPI",
        Version = "v1",
        Description = "API de catálogo musical, usuários, playlists e avaliações."
    });

    options.InferSecuritySchemes();
});

builder.Services.AddCors(options =>
    options.AddPolicy("FrontEnd", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "CatalogoMusica.WebAPI v1"
        );

        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors("FrontEnd");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();