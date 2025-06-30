using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivelab.Servicios.Interfaces
{
    public interface IMetodoPagoService
    {
        Task<bool> ElejirMetodoPago(int metodoPagoCodigo);
    }
}
