using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vivelab.Modelos;

public class Album
{
    [Key]
    public int Code { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public string? CoverUrl { get; set; }

    public int ArtistCode { get; set; }

    public virtual User? Artist { get; set; }

    public virtual List<Song>? Songs { get; set; }
}
