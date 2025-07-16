using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vivelab.Modelos;

public class Song
{
    [Key]
    public int Code { get; set; }

    public string Title { get; set; } = null!;

    public TimeSpan Duration { get; set; }

    public string FileUrl { get; set; } = null!;

    public DateTime UploadDate { get; set; }

    public int TotalPlays { get; set; }

    public string? CoverUrl { get; set; }

    public int ArtistCode { get; set; }

    public int? AlbumCode { get; set; }

    public virtual Album? Album { get; set; }

    public virtual User? Artist { get; set; } 

    public virtual List<PlaylistSong>? PlayListsSongs { get; set; }
}
