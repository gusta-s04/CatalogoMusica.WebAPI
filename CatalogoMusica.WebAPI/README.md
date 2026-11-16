# CatalogoMusica.WebAPI

API ASP.NET Core baseada na organização do Plot.WebAPI e no banco `CatalogoMusicasDB`.

## Configuração
1. Execute `CatalogoMusica.sql` no SQL Server.
2. Abra `appsettings.json` e ajuste `ConnectionStrings:DefaultConnection` para seu servidor.
3. Altere `Jwt:Key` para uma chave secreta própria com pelo menos 32 bytes.
4. Restaure os pacotes e execute o projeto.

```bash
dotnet restore
dotnet run
```

O Swagger abre na raiz da aplicação em ambiente Development.

## Administrador inicial
- E-mail: `admin@catalogomusicas.com`
- Senha: `admin123`

Na inicialização, o projeto cria o administrador caso não exista. Se encontrar o hash SHA2_256 legado gerado pelo SQL original, valida que ele corresponde à senha inicial `admin123` e o substitui por BCrypt. Novas senhas são sempre armazenadas com BCrypt.

## Endpoints principais
- `POST /api/Login`
- `/api/Usuario`
- `/api/Artista`
- `/api/Album`
- `/api/Genero`
- `/api/Musica`
- `/api/MusicaGenero`
- `/api/Playlist`
- `/api/PlaylistMusica`
- `/api/Favoritos`
- `/api/Avaliacao`
- `/api/Comentario`

Operações administrativas usam a role `Admin`. Operações de playlists, avaliações e comentários exigem autenticação. Para produção, ajuste CORS e segredos conforme seu ambiente.
