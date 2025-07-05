using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivelab.Servicios.Interfaces
{
    public interface IPerfilService
    {
        Task<bool> CambiarRolUsuario(string email); // Cambia el rol del usuario (por ejemplo, de usuario a administrador)
        Task<bool> CambiarPassword(string email, string passwordActual, string passwordNuevo); // Cambia la contraseña del usuario
        Task<bool> EliminarCuenta(string email); // Elimina la cuenta del usuario
        Task<bool> ActualizarNombre(string email, string nombre); // Actualiza los datos del perfil del usuario
    }
}
