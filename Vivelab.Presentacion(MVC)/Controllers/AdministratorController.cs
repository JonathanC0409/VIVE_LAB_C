using Microsoft.AspNetCore.Mvc;
using Vivelab.API.Consume;
using Vivelab.Modelos;

namespace Vivelab.Presentacion_MVC_.Controllers
{
    public class AdministratorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        // GET: UsuarioController/Usuarios
        public ActionResult ViewUser()
        {
            // Obtener todos los usuarios
            var usuarios = CRUD<User>.GetAll();

            // Pasar los usuarios a la vista
            return View(usuarios);
        }

        // GET: UsuarioController/BloquearUsuario
        public ActionResult BlockUser()
        {
            var usuarios = CRUD<User>.GetAll();

            // Pasar la lista de usuarios a la vista
            return View(usuarios);
        }


        // POST: UsuarioController/BloquearUsuario
        [HttpPost]
        public IActionResult BlockUser(int usuarioId)
        {
            // Verificar si el usuario existe
            var usuario = CRUD<User>.GetById(usuarioId);
            if (usuario == null)
            {
                return NotFound();
            }


            usuario.Role = "bloqueado";
            CRUD<User>.Update(usuarioId, usuario);

            return RedirectToAction("Index", "Home");
        }

        // GET: UsuarioController/EliminarCancion/5
        public ActionResult deleteMusic(int id)
        {
            var cancion = CRUD<Song>.GetById(id); // Obtener la canción por su id
            if (cancion == null) return NotFound();

            return View(cancion); // Pasar la canción a la vista para confirmar su eliminación
        }

        // POST: UsuarioController/EliminarCancion/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult deleteMusic(int id, IFormCollection collection)
        {
            try
            {
                var cancion = CRUD<Song>.GetById(id);
                if (cancion == null) return NotFound();

                CRUD<Song>.Delete(id); // Eliminar la canción

                return RedirectToAction("MisCanciones"); // Redirigir al listado de canciones
            }
            catch
            {
                return View();
            }
        }
    }
}
