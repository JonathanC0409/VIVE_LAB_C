using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vivelab.API.Consume;
using Vivelab.Modelos;
using Vivelab.Servicios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Vivelab.Presentacion_MVC_.Controllers
{
    [Authorize]
    public class GestionarPerfilController : Controller
    {
        private readonly IPerfilService _perfilService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GestionarPerfilController(IPerfilService perfilService, IHttpContextAccessor httpContextAccessor)
        {
            _perfilService = perfilService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            ViewBag.Email = email;
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarNombre(string email, string nombre)
        {
            // 1. Actualiza el nombre en la base de datos
            var resultado = await _perfilService.ActualizarNombre(email, nombre);

            if (resultado)
            {
                // 2. Obtener el usuario actualizado desde la base de datos
                var usuario = CRUD<Usuario>.GetAll().FirstOrDefault(u => u.Email == email);

                if (usuario != null)
                {
                    // 3. Crear una nueva identidad de Claims con el nuevo nombre
                    var datosUsuario = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Nombre), // Nombre actualizado
                        new Claim(ClaimTypes.Email, usuario.Email), // Correo electrónico (sin cambios)
                        new Claim("TipoUsuario", usuario.TipoUsuario) // Tipo de usuario (sin cambios)
                    };

                    var credencialesActualizadas = new ClaimsIdentity(datosUsuario, "Cookies");
                    var usuarioAutenticado = new ClaimsPrincipal(credencialesActualizadas);

                    // 4. Actualizar los claims en la sesión activa
                    await _httpContextAccessor.HttpContext.SignInAsync("Cookies", usuarioAutenticado);

                    TempData["Mensaje"] = "Perfil actualizado exitosamente.";
                    return RedirectToAction("Index"); // Redirigir al perfil o a la página principal
                }
            }

            TempData["Mensaje"] = "Error al actualizar el perfil. Por favor, inténtelo de nuevo.";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> CambiarPassword(string email, string passwordActual, string passwordNuevo)
        {

            var resultado = await _perfilService.CambiarPassword(email, passwordActual, passwordNuevo);
            if (resultado)
            {
                ViewBag.Mensaje = "Contraseña cambiada exitosamente.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensaje = "Error al cambiar la contraseña. Por favor, inténtelo de nuevo.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult CambiarRolUsuario()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CambiarRolUsuario(string email)
        {
            var resultado = await _perfilService.CambiarRolUsuario(email);
            if (resultado)
            {
                ViewBag.Mensaje = "Rol de usuario cambiado exitosamente.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensaje = "Error al cambiar el rol del usuario. Por favor, inténtelo de nuevo.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCuenta(string email)
        {
            var resultado = await _perfilService.EliminarCuenta(email);
            if (resultado)
            {
                ViewBag.Mensaje = "Cuenta eliminada exitosamente.";


                await HttpContext.SignOutAsync("Cookies");
                return RedirectToAction("Index", "Home");
            }

            else
            {
                ViewBag.Mensaje = "Error al eliminar la cuenta. Por favor, inténtelo de nuevo.";
                return RedirectToAction("Index");
            }

        }
    }
}
