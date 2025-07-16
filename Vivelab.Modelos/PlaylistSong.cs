using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vivelab.Modelos;

public class PlaylistSong
{
    [Key]
    public int Code { get; set; }

    public int PlaylistCode { get; set; }

    public int SongCode { get; set; }

    public virtual Song? Song { get; set; }

    public virtual Playlist? Playlist { get; set; }
}
