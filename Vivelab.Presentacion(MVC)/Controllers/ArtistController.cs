using Microsoft.AspNetCore.Mvc;
using Vivelab.API.Consume;
using Vivelab.Modelos;

namespace Vivelab.Presentacion_MVC_.Controllers
{
    public class ArtistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: UsuarioController/MisCanciones
        public ActionResult ManageSongs()
        {
            int id = 0;
            foreach (var u in User.Claims)
            {
                if (u.Type == "UsuarioCodigo")
                {
                    id = int.Parse(u.Value); // Obtener el id del usuario logueado
                }
            }

            // Obtener las canciones del artista desde la base de datos
            var usuario = CRUD<User>.GetById(id);
            if (usuario == null) return NotFound();

            // Suponiendo que Usuario tiene una lista de canciones
            var canciones = usuario.Songs;
            return View(canciones); // Pasar las canciones a la vista
        }

        // GET: UsuarioController/EliminarCancion/5
        public ActionResult DeleteSong(int id)
        {
            var cancion = CRUD<Song>.GetById(id); // Obtener la canción por su id
            if (cancion == null) return NotFound();

            return View(cancion); // Pasar la canción a la vista para confirmar su eliminación
        }

        // POST: UsuarioController/EliminarCancion/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSong(int id, Song song)
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
