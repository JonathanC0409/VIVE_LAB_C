using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vivelab.Modelos;

public class Usuario
{
    [Key]
    public int Codigo { get; set; }

    public string Nombre { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Rol { get; set; }

    public string? Bibliografia { get; set; }

    public double Saldo { get; set; }

    public DateTime FechaRegistro { get; set; }

    public  List<Album>? Albums { get; set; }

    public  List<Cancion>? Canciones { get; set; }

    public  List<Playlist>? Playlists { get; set; }

    public  Suscripcion? Suscripcion { get; set; }

    public  List<UsuarioSuscripcion>? UsuariosSuscripciones { get; set; }
}
