using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vivelab.Modelos;

public class User
{
    [Key]
    public int Code { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Role { get; set; }

    public string? Bibliography { get; set; }

    public double Balance { get; set; }

    public DateTime RegistrationDate { get; set; }

    public virtual List<Album>? Albums { get; set; }

    public virtual List<Song>? Songs { get; set; }

    public virtual List<Playlist>? Playlists { get; set; }

    public virtual Subscription? Subscription { get; set; }

    public virtual List<UserSubscription>? UsersSubscriptions { get; set; }
}
