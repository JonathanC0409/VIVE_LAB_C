using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vivelab.Api;
using Vivelab.API.Consume;
using Vivelab.Modelos;
using Vivelab.Servicios.Interfaces;


namespace Vivelab.Servicios
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor; // Para acceder al contexto HTTP y manejar la autenticación
        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Login(string email, string password)
        {

            var usuarios = CRUD<Usuario>.GetAll();
            foreach (var usuario in usuarios)
            {
                if (usuario.Email == email)
                {

                    Console.WriteLine($"Comparadno pas ingrasado {password} con contraseña guardada {usuario.Password} ");
                    if (BCrypt.Net.BCrypt.Verify(password, usuario.Password))
                    {
                        var datosUsuario = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuario.Nombre),
                            new Claim(ClaimTypes.Email, usuario.Email),
                            new Claim("TipoUsuario", usuario.TipoUsuario)
                        };
                        var credencialDigital = new ClaimsIdentity(datosUsuario, "Cookies");
                        var usuarioAutenticado = new ClaimsPrincipal(credencialDigital);

                        await _httpContextAccessor.HttpContext.SignInAsync("Cookies", usuarioAutenticado);
                        return true;
                    }
                }
            }
            return false; // Usuario no encontrado o contraseña incorrecta


        }

        public async Task<bool> Register(string email, string nombre, string password)
        {
            var usuarioExistente = CRUD<Usuario>.GetAll()
                .FirstOrDefault(u => u.Email == email);
            if (usuarioExistente != null)
            {
                return false; // El usuario ya existe   
            }

            try
            {
                CRUD<Usuario>.Create(new Usuario
                {
                    Codigo = 0,
                    Email = email,
                    Password = password, // Aquí deberías aplicar un hash a la contraseña antes de guardarla
                    Nombre = nombre,
                    TipoUsuario = "cliente",
                    FechaRegistro = DateTime.UtcNow
                });
                return true; // Registro exitoso
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar usuario: {ex.Message}");
                return false; // Error al registrar usuario

            }
        }

      
    }
}
