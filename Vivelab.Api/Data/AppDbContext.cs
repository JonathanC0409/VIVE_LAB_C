using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vivelab.Modelos;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vivelab.Modelos.Album> Albumes { get; set; } = default!;

    public DbSet<Vivelab.Modelos.Cancion> Canciones { get; set; } = default!;

    public DbSet<Vivelab.Modelos.MetodoPago> MetodoPagos { get; set; } = default!;

    public DbSet<Vivelab.Modelos.Plan> Planes { get; set; } = default!;

    public DbSet<Vivelab.Modelos.Playlist> Playlists { get; set; } = default!;

    public DbSet<Vivelab.Modelos.PlaylistCancion> PlaylistCanciones { get; set; } = default!;

    public DbSet<Vivelab.Modelos.Suscripcion> Suscripciones { get; set; } = default!;

    public DbSet<Vivelab.Modelos.Usuario> Usuarios { get; set; } = default!;

    public DbSet<Vivelab.Modelos.UsuarioSuscripcion> UsuariosSuscripciones { get; set; } = default!;
}
