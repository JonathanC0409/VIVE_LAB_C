using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using Vivelab.API.Consume;
using Vivelab.Modelos;
using Vivelab.Presentacion_MVC_.Models;

namespace Vivelab.Presentacion_MVC_.Controllers
{
    public class MusicaController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            
            ViewBag.Rol = Rol();
            int planCodigo = ObtenerPlan();
            ViewBag.PlanCodigo = planCodigo;

            int usuarioId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UsuarioCodigo")?.Value ?? "0");

            if (planCodigo == 0)
            {
                string key = $"Reproducciones_{usuarioId}_{DateTime.UtcNow:yyyyMMdd}";
                int reproducciones = HttpContext.Session.GetInt32(key) ?? 0;

                ViewBag.ReproduccionesHoy = reproducciones;
                ViewBag.MostrarAnuncios = true;
                ViewBag.PuedeReproducir = reproducciones < 10;
            }
            else
            {
                ViewBag.ReproduccionesHoy = -1;
                ViewBag.PuedeReproducir = true;
            }

            // Descargas
            string dkey = $"Descargas_{usuarioId}_{DateTime.UtcNow:yyyyMMdd}";
            int descargasHoy = HttpContext.Session.GetInt32(dkey) ?? 0;
            int limite = planCodigo switch
            {
                0 => 0,
                1 => 1,
                2 => 10,
                3 => 100,
                _ => 0
            };

            ViewBag.DescargasHoy = descargasHoy;
            ViewBag.LimiteDescargas = limite;

            var lista = CRUD<Cancion>.GetAll();
            return View(lista);
        }


        private string Rol()
        {
            string rol = "";
            foreach (var u in User.Claims)
            {
                if (u.Type == "TipoUsuario")
                {
                    rol = u.Value;
                }
            }
            return rol;
        }

        private int ObtenerPlan()
        {
            int UsuarioId = 0;
            foreach (var u in User.Claims)
            {
                if (u.Type == "UsuarioCodigo")
                {
                    UsuarioId = int.Parse(u.Value);
                    break;
                }
            }

            // Obtener el usuario principal (logueado)
            var usuario = CRUD<Usuario>.GetById(UsuarioId);
            if (usuario != null)
            {
                // Si tiene una suscripción activa, se retorna el plan
                if (usuario.Suscripcion != null)
                {
                    return usuario.Suscripcion.Plan.Codigo;
                }

                // Si no tiene una suscripción activa, se busca en las suscripciones de otros usuarios si lo tienen vinculado
                var usuarioSubcripciones = CRUD<UsuarioSuscripcion>.GetAll();
                foreach (var u in usuarioSubcripciones)
                {
                    if (usuario.Codigo == u.UsuarioCodigo)
                    {
                        return u.Suscripcion.PlanCodigo;
                    }
                }

            }
            return 0;
        }

        [HttpGet]
        public IActionResult Subir() => View(new CancionUploadViewModel());

        [HttpPost]
        public IActionResult Subir(CancionUploadViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // abre el stream y llama al CRUD
            var cancion = CRUD<Cancion>.UploadWithFile(
                vm.Titulo,
                vm.Archivo.OpenReadStream(),
                vm.Archivo.FileName,
                vm.Archivo.ContentType,
                vm.Duracion,
                vm.ArtistaCodigo,
                vm.AlbumCodigo
            );

            // redirige o maneja la respuesta
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult IncrementarReproduccionUsuario()
        {
            int usuarioId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UsuarioCodigo")?.Value ?? "0");

            string key = $"Reproducciones_{usuarioId}_{DateTime.UtcNow:yyyyMMdd}";
            int reproducciones = HttpContext.Session.GetInt32(key) ?? 0;

            HttpContext.Session.SetInt32(key, reproducciones + 1);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Descargar(int cancionId)
        {
            int usuarioId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UsuarioCodigo")?.Value ?? "0");
            int plan = ObtenerPlan();  // Aquí ya estamos obteniendo el plan, incluso si el usuario está vinculado.

            string key = $"Descargas_{usuarioId}_{DateTime.UtcNow:yyyyMMdd}";
            int descargasHoy = HttpContext.Session.GetInt32(key) ?? 0;

            int limite = plan switch
            {
                0 => 0,  // No tiene plan
                1 => 1,  // Límite para plan básico
                2 => 10, // Límite para plan estándar
                3 => 100, // Límite para plan premium
                _ => 0
            };
            if (plan == 0)
                return BadRequest("Tu plan no permite descargas.");

            if (descargasHoy >= limite)
                return BadRequest("Has alcanzado el límite diario de descargas.");

            var cancion = CRUD<Cancion>.GetById(cancionId);
            if (cancion == null) return NotFound();

            var httpClient = new HttpClient();
            var archivoBytes = await httpClient.GetByteArrayAsync(cancion.ArchivoUrl);

            HttpContext.Session.SetInt32(key, descargasHoy + 1);

            return File(archivoBytes, "audio/mpeg", $"{cancion.Titulo}.mp3");
        }

    }
}

