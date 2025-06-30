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
    public class PerfilService : IPerfilService
    {
        public Task<bool> ActualizarNombre(string email, string nombre)
        {
            var usuario = CRUD<Usuario>.GetAll().FirstOrDefault(u => u.Email == email);
            if (usuario != null)
            {
                usuario.Nombre = nombre;
                Console.WriteLine($"Perfil passow {usuario.Password}");
                CRUD<Usuario>.Update(usuario.Codigo, usuario);
                Console.WriteLine($"Perfil del usuario con email {email} actualizado a nombre {nombre}. password {usuario.Password}");
                return Task.FromResult(true);
            }
            else
            {
                Console.WriteLine($"Usuario con email {email} no encontrado.");
                return Task.FromResult(false);
            }
        }

        public Task<bool> CambiarPassword(string email, string passwordActual, string passwordNuevo)
        {
            var usuario = CRUD<Usuario>.GetAll().FirstOrDefault(u => u.Email == email);
            if (usuario != null)
            {
                if (BCrypt.Net.BCrypt.Verify(passwordActual,usuario.Password))
                {
                    usuario.Password = passwordNuevo; 
                    CRUD<Usuario>.Update(usuario.Codigo, usuario);
                    Console.WriteLine($"Contraseña del usuario con email {email} cambiada exitosamente."); 
                    return Task.FromResult(true); // Contraseña cambiada exitosamente
                }
                else
                {
                    Console.WriteLine("La contraseña actual es incorrecta.");
                    return Task.FromResult(false);
                }
            }
            else
            {
                Console.WriteLine($"Usuario con email {email} no encontrado.");
                return Task.FromResult(false);
            }
        }

        public async Task<bool> CambiarRolUsuario(string email)
        {
            var usuarios = CRUD<Usuario>.GetAll();
            foreach(var usuario in usuarios)
            {
                if(usuario.Email == email && usuario.TipoUsuario == "cliente")
                {
                    usuario.TipoUsuario = "artista"; 
                    CRUD<Usuario>.Update(usuario.Codigo, usuario);
                    Console.WriteLine($"Rol del usuario con email {email} cambiado a artista.");
                    return true; // Rol cambiado exitosamente
                }
            }

            Console.WriteLine($"Usuario con email {email} no encontrado o ya es artista.");
            return false;
                  
        }

        public Task<bool> EliminarCuenta(string email)
        {
            CRUD<Usuario>.GetAll().ForEach(u =>
            {
                if (u.Email == email)
                {
                    CRUD<Usuario>.Delete(u.Codigo);
                    Console.WriteLine($"Cuenta del usuario con email {email} eliminada.");
                }
            });
            return Task.FromResult(true); // Cuenta eliminada exitosamente
        }
    }
}
