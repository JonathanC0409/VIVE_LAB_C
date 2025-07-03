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

    public virtual List<PlaylistCancion>? PlaylistCanciones { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
