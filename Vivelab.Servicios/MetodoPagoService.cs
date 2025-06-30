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
    public class MetodoPagoService : IMetodoPagoService
    {
        public Task<bool> ElejirMetodoPago(int metodoPagoCodigo)
        {
            var metodoPago = CRUD<MetodoPago>.GetById(metodoPagoCodigo);
            return Task.FromResult(metodoPago != null);
        }
    }
}
