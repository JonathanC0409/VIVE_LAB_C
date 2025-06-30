using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivelab.Modelos;

namespace Vivelab.Servicios.Interfaces
{
    public interface ISuscripcionService
    {
        Task<bool> CrearSuscripcion(Suscripcion suscripcion);
    }
}
