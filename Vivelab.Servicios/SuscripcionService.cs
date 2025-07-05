using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivelab.API.Consume;
using Vivelab.Modelos;
using Vivelab.Servicios.Interfaces;

namespace Vivelab.Servicios
{
    public class SuscripcionService : ISuscripcionService
    {
        public Task<bool> CrearSuscripcion(Suscripcion suscripcion)
        {
            if(suscripcion == null)
            {
                return Task.FromResult(false);
            }
            else
            {
                try
                {
                    var response = CRUD<Suscripcion>.Create(suscripcion);
                    return Task.FromResult(response != null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear la suscripción: {ex.Message}");
                    return Task.FromResult(false);
                }
            }
        }
    }
}
