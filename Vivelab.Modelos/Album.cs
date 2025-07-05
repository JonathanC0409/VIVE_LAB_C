using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vivelab.Modelos;

public class Album
{
    [Key]
    public int Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public string? PortadaUrl { get; set; }

    public int ArtistaCodigo { get; set; }

    public virtual Usuario? Artista { get; set; }

    public virtual List<Cancion>? Canciones { get; set; }
}
