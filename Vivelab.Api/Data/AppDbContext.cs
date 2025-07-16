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

    public DbSet<Vivelab.Modelos.Album> Albums { get; set; } = default!;

    public DbSet<Vivelab.Modelos.Song> Songs { get; set; } = default!;

    public DbSet<Vivelab.Modelos.Plan> Plans { get; set; } = default!;

    public DbSet<Vivelab.Modelos.Playlist> Playlists { get; set; } = default!;

    public DbSet<Vivelab.Modelos.PlaylistSong> PlaylistSongs { get; set; } = default!;

    public DbSet<Vivelab.Modelos.Subscription> Subscriptions { get; set; } = default!;

    public DbSet<Vivelab.Modelos.User> Users { get; set; } = default!;

    public DbSet<Vivelab.Modelos.UserSubscription> UsersSubscriptions { get; set; } = default!;
}
