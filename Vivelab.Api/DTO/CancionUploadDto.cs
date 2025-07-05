using System.ComponentModel.DataAnnotations;

namespace Vivelab.Api.DTO
{
    public class CancionUploadDto
    {
        [Required] public string Titulo { get; set; } = null!;
        [Required] public IFormFile Archivo { get; set; } = null!;
        public TimeSpan Duracion { get; set; }
        [Required] public int ArtistaCodigo { get; set; }
        [Required] public int AlbumCodigo { get; set; }
    }
}
