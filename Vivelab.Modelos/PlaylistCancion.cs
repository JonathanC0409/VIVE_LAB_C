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

    public  Cancion? Cancion { get; set; }

    public Playlist? Playlist { get; set; }
}
