using System.ComponentModel.DataAnnotations;

namespace Vivelab.Presentacion_MVC_.Models
{
    public class CancionUploadViewModel
    {
        [Required] public string Titulo { get; set; } = null!;
        [Required] public IFormFile Archivo { get; set; } = null!;
        public TimeSpan Duracion { get; set; }
        [Required] public int ArtistaCodigo { get; set; }
        [Required] public int AlbumCodigo { get; set; }
    }

}
