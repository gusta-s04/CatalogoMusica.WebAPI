
using CatalogoMusica.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMusica.WebAPI.Context;

public class CatalogoMusicaDbContext : DbContext
{
    public CatalogoMusicaDbContext(DbContextOptions<CatalogoMusicaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuario => Set<Usuario>();
    public DbSet<Artista> Artista => Set<Artista>();
    public DbSet<Album> Album => Set<Album>();
    public DbSet<Genero> Genero => Set<Genero>();
    public DbSet<Musica> Musica => Set<Musica>();
    public DbSet<MusicaGenero> MusicaGenero => Set<MusicaGenero>();
    public DbSet<Playlist> Playlist => Set<Playlist>();
    public DbSet<PlaylistMusica> PlaylistMusica => Set<PlaylistMusica>();
    public DbSet<Favorito> Favoritos => Set<Favorito>();
    public DbSet<Avaliacao> Avaliacao => Set<Avaliacao>();
    public DbSet<Comentario> Comentario => Set<Comentario>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Usuario>(e =>
        {
            e.ToTable("Usuario", t => t.HasTrigger("TR_UsuarioSoftDelete"));

            e.HasKey(x => x.IdUsuario);

            e.HasIndex(x => x.Email).IsUnique();

            e.Property(x => x.Nome)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.Email)
                .HasMaxLength(50)
                .IsRequired();

            e.Property(x => x.Senha)
                .HasMaxLength(255)
                .IsRequired();

            e.Property(x => x.TipoUsuario)
                .HasMaxLength(20)
                .IsRequired();

            e.Property(x => x.Ativo)
                .HasDefaultValue(true);
        });

        b.Entity<Artista>(e =>
        {
            e.ToTable("Artista");

            e.HasKey(x => x.IdArtista);

            e.Property(x => x.Nome)
                .HasMaxLength(100)
                .IsRequired();
        });

        b.Entity<Album>(e =>
        {
            e.ToTable("Album");

            e.HasKey(x => x.IdAlbum);

            e.Property(x => x.Nome)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.CapaUrl)
                .HasMaxLength(255);

            e.HasOne(x => x.Artista)
                .WithMany(x => x.Albuns)
                .HasForeignKey(x => x.IdArtista)
                .OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Genero>(e =>
        {
            e.ToTable("Genero");

            e.HasKey(x => x.IdGenero);

            e.HasIndex(x => x.Nome).IsUnique();

            e.Property(x => x.Nome)
                .HasMaxLength(50)
                .IsRequired();
        });

        b.Entity<Musica>(e =>
        {
            e.ToTable("Musica");

            e.HasKey(x => x.IdMusica);

            e.Property(x => x.Nome)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.URLAudio)
                .HasMaxLength(255);

            e.Property(x => x.CapaUrl)
                .HasMaxLength(255);

            e.HasOne(x => x.Artista)
                .WithMany(x => x.Musicas)
                .HasForeignKey(x => x.IdArtista)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Album)
                .WithMany(x => x.Musicas)
                .HasForeignKey(x => x.IdAlbum)
                .OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<MusicaGenero>(e =>
        {
            e.ToTable("MusicaGenero");

            e.HasKey(x => new { x.IdMusica, x.IdGenero });

            e.HasOne(x => x.Musica)
                .WithMany(x => x.MusicasGeneros)
                .HasForeignKey(x => x.IdMusica);

            e.HasOne(x => x.Genero)
                .WithMany(x => x.MusicasGeneros)
                .HasForeignKey(x => x.IdGenero);
        });

        b.Entity<Playlist>(e =>
        {
            e.ToTable("Playlist");

            e.HasKey(x => x.IdPlaylist);

            e.Property(x => x.Nome)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.DataCriacao)
                .HasDefaultValueSql("GETDATE()");

            e.HasOne(x => x.Usuario)
                .WithMany(x => x.Playlists)
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<PlaylistMusica>(e =>
        {
            e.ToTable("PlaylistMusica");

            e.HasKey(x => new { x.IdPlaylist, x.IdMusica });

            e.Property(x => x.DataAdicao)
                .HasDefaultValueSql("GETDATE()");

            e.HasOne(x => x.Playlist)
                .WithMany(x => x.Musicas)
                .HasForeignKey(x => x.IdPlaylist);

            e.HasOne(x => x.Musica)
                .WithMany(x => x.PlaylistsMusicas)
                .HasForeignKey(x => x.IdMusica);
        });

        b.Entity<Favorito>(e =>
        {
            e.ToTable("Favoritos");

            e.HasKey(x => new { x.IdUsuario, x.IdMusica });

            e.Property(x => x.DataFavorito)
                .HasDefaultValueSql("GETDATE()");

            e.HasOne(x => x.Usuario)
                .WithMany(x => x.Favoritos)
                .HasForeignKey(x => x.IdUsuario);

            e.HasOne(x => x.Musica)
                .WithMany(x => x.Favoritos)
                .HasForeignKey(x => x.IdMusica);
        });

        b.Entity<Avaliacao>(e =>
        {
            e.ToTable("Avaliacao");

            e.HasKey(x => x.IdAvaliacao);

            e.HasIndex(x => new { x.IdUsuario, x.IdMusica })
                .IsUnique();

            e.Property(x => x.DataAvaliacao)
                .HasDefaultValueSql("GETDATE()");

            e.HasOne(x => x.Musica)
                .WithMany(x => x.Avaliacoes)
                .HasForeignKey(x => x.IdMusica);

            e.HasOne(x => x.Usuario)
                .WithMany(x => x.Avaliacoes)
                .HasForeignKey(x => x.IdUsuario);
        });

        b.Entity<Comentario>(e =>
        {
            e.ToTable("Comentario");

            e.HasKey(x => x.IdComentario);

            e.Property(x => x.DataComentario)
                .HasDefaultValueSql("GETDATE()");

            e.HasOne(x => x.Musica)
                .WithMany(x => x.Comentarios)
                .HasForeignKey(x => x.IdMusica);

            e.HasOne(x => x.Usuario)
                .WithMany(x => x.Comentarios)
                .HasForeignKey(x => x.IdUsuario);
        });
    }
}
