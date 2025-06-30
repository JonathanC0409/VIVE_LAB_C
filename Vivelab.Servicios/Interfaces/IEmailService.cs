using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivelab.Servicios.Interfaces
{
    public interface IEmailService
    {
        Task enviarEmailBienvenida(string email); // Envía un correo electrónico de bienvenida al usuario
        Task enviarEmailRecuperacionPassword(string email); // Envía un correo electrónico de recuperación de contraseña al usuario
    }
}
