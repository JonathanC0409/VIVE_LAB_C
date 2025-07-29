using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vivelab.Modelos;

public class Playlist
{
    [Key]
    public int Codigo { get; set; }

    public string Nombre { get; set; }

    public int UsuarioCodigo { get; set; }

    public  List<PlaylistCancion>? PlaylistCanciones { get; set; }

    public  Usuario? Usuario { get; set; }
}
