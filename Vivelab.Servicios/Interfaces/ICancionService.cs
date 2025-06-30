using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivelab.Modelos;

namespace Vivelab.Servicios.Interfaces
{
    public interface ICancionService
    {
        Task<Cancion> SubirCancion(
            string titulo,
            Stream fileStream,
            string fileName,
            string contentType,
            TimeSpan duracion,
            int artistaCodigo,
            int albumCodigo
        );
    }
}
