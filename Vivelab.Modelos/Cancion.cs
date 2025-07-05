using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vivelab.Modelos;

public class Cancion
{
    [Key]
    public int Codigo { get; set; }

    public string Titulo { get; set; } = null!;

    public TimeSpan Duracion { get; set; }

    public string ArchivoUrl { get; set; } = null!;

    public DateTime FechaSubida { get; set; }

    public int TotalReproducciones { get; set; }
    public string? PortadaUrl { get; set; }

    public int ArtistaCodigo { get; set; }

    public int? AlbumCodigo { get; set; }

    public virtual Album? Album { get; set; }



    public virtual Usuario? Artista { get; set; } 


    public virtual List<PlaylistCancion>? PlaylistCanciones { get; set; }
}
