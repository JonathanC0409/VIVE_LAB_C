// En Vivelab.Servicios/CancionService.cs
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Vivelab.Modelos;
using Vivelab.Servicios.Interfaces;

namespace Vivelab.Servicios
{
    public class CancionService : ICancionService
    {
        private readonly BlobServiceClient _blobService;
        private readonly string _containerName;

        // Constructor donde se inyecta el BlobServiceClient y el nombre del contenedor
        public CancionService(BlobServiceClient blobService, string containerName)
        {
            _blobService = blobService;
            _containerName = containerName;
        }

        // Método para subir la canción
        public async Task<Cancion> SubirCancion(string titulo, Stream fileStream, string fileName, string contentType, TimeSpan duracion, int artistaCodigo, int albumCodigo)
        {
            // 1. Obtener el contenedor de blobs
            var container = _blobService.GetBlobContainerClient(_containerName);

            // 2. Crear el contenedor si no existe
            await container.CreateIfNotExistsAsync();

            // 3. Generar un nombre único para el archivo
            var blobName = $"{Guid.NewGuid()}_{fileName}";
            var blobClient = container.GetBlobClient(blobName);

            // 4. Subir el archivo
            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });

            // 5. Crear una nueva entidad Cancion con la URL del archivo subido
            var cancion = new Cancion
            {
                Titulo = titulo,
                ArchivoUrl = blobClient.Uri.ToString(),
                Duracion = duracion,
                FechaSubida = DateTime.UtcNow,
                ArtistaCodigo = artistaCodigo,
                AlbumCodigo = albumCodigo,
                TotalReproducciones = 0 // Iniciar con 0 reproducciones
            };

            // Aquí se debe agregar la canción a la base de datos (esto depende de tu contexto de EF)
            // _context.Canciones.Add(cancion);
            // await _context.SaveChangesAsync();

            return cancion; // Retorna la canción agregada
        }
    }
}
