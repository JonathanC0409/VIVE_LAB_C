using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Vivelab.API.Consume;
using Vivelab.Modelos;
using Vivelab.Servicios.Interfaces;

namespace Vivelab.Presentacion_MVC_.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        public LoginController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (await _authService.Login(email, password))
            {
                // Enviar correo electrónico de bienvenida
                await _emailService.enviarEmailBienvenida(email);
                // Redirigir a la página principal o dashboard
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Mostrar mensaje de error
                ViewBag.ErrorMessage = "Email o contraseña incorrectos.";
                return View("Index");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string nombre, string password)
        {
            if (await _authService.Register(email, nombre, password))
            {
                // Redirigir a la página de inicio de sesión
                return RedirectToAction("Index", "Login");
            }
            else
            {
                // Mostrar mensaje de error
                ViewBag.ErrorMessage = "Error al registrar el usuario.";
                return View();
            }
        }
        [HttpGet]
        public IActionResult RecuperarPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RecuperarPassword(string email)
        {
            var usuario = CRUD<Usuario>.GetAll().FirstOrDefault(u => u.Email == email);
            if (usuario == null)
            {
                ViewBag.ErrorMessage = "El correo electrónico no está registrado.";
                return View("Index");
            }
            // Enviar correo electrónico de recuperación de contraseña
            await _emailService.enviarEmailRecuperacionPassword(email);
            ViewBag.SuccessMessage = "Se ha enviado un correo electrónico para recuperar la contraseña.";
            return RedirectToAction("Index", "Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Login");
        }
    }
}
