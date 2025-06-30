using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vivelab.Modelos;

public class PlaylistCancion
{
    [Key]
    public int Codigo { get; set; }

    public int PlaylistCodigo { get; set; }

    public int CancionCodigo { get; set; }

    public virtual Cancion? Cancion { get; set; }

    public virtual Playlist? Playlist { get; set; } 
}
