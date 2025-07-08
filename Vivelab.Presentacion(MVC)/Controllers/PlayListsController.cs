using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vivelab.API.Consume;
using Vivelab.Modelos;

namespace Vivelab.Presentacion_MVC_.Controllers
{
    public class PlayListsController : Controller
    {
        // GET: PlayListsController
        public ActionResult Index()
        {

            try
            {
                var usuarioCodigo = 0;
                foreach (var u in User.Claims)
                {
                    if (u.Type == "UsuarioCodigo")
                    {
                        usuarioCodigo = int.Parse(u.Value);
                    }
                }
                ViewBag.UsuarioCodigo = usuarioCodigo;
                ViewBag.Plan = ObtenerPlan();
                // Obtener todas las playlists usando el CRUD
                var playlists = CRUD<Playlist>.GetBy("usuario", usuarioCodigo);
                return View(playlists); // Mostrar todas las playlists
            }
            catch (Exception ex)
            {
                // Si ocurre un error, mostrar mensaje
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
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

            var usuario = CRUD<Usuario>.GetById(UsuarioId);
            if (usuario != null)
            {
                if (usuario.Suscripcion == null)
                {
                    return 0; // No tiene plan
                }
                int plan = usuario.Suscripcion.Plan.Codigo;
                return plan;
            }
            return 0;
        }

        // GET: PlayListsController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                // Obtener los detalles de una playlist usando el CRUD
                var playlist = CRUD<Playlist>.GetById(id);
                return View(playlist); // Mostrar detalles de la playlist
            }
            catch (Exception ex)
            {
                // Si ocurre un error, mostrar mensaje
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: PlayListsController/Create
        public ActionResult Create()
        {
            int plan = ObtenerPlan();

            if (plan == 0)
            {
                TempData["Mensaje"] = "Tu plan no permite crear playlists. Actualiza tu plan para acceder.";
                return RedirectToAction("Index", "Plan");
            }

            int usuarioCodigo = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UsuarioCodigo")?.Value ?? "0");
            ViewBag.UsuarioCodigo = usuarioCodigo;
            return View();
        }

        // POST: PlayListsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Playlist playlist)
        {
            try
            {

                CRUD<Playlist>.Create(playlist); // Crear una nueva playlist usando el CRUD
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: PlayListsController/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                // Obtener la playlist a editar usando el CRUD
                var playlist = CRUD<Playlist>.GetById(id);

                if (playlist == null)
                {
                    return NotFound();
                }

                // Cargar las canciones asociadas a la playlist desde la relación PlaylistCancion
                var playlistCanciones = CRUD<PlaylistCancion>.GetBy("playlist", id);  // Esto obtiene las canciones asociadas a la playlist

                // Obtener las canciones de la playlist
                var cancionesEnPlaylist = playlistCanciones.Select(pc => pc.Cancion).ToList();

                // Pasar las canciones disponibles a la vista
                ViewBag.AllSongs = cancionesEnPlaylist; // Mostrar las canciones asociadas a la playlist en la vista

                return View(playlist); // Mostrar la vista de edición con los datos de la playlist
            }
            catch (Exception ex)
            {
                // Si ocurre un error, mostrar mensaje
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, List<int> Canciones)
        {
            try
            {
                Console.WriteLine($"Inicio del proceso para editar la playlist con ID: {id}");

                // Obtener la playlist a editar usando el CRUD
                var playlist = CRUD<Playlist>.GetById(id);
                Console.WriteLine($"Playlist obtenida: {playlist?.Nombre}");

                if (playlist == null)
                {
                    Console.WriteLine("Playlist no encontrada.");
                    return NotFound(); // Si la playlist no existe
                }

                // Verificar que PlaylistCanciones no sea null
                if (playlist.PlaylistCanciones == null)
                {
                    playlist.PlaylistCanciones = new List<PlaylistCancion>(); // Inicializar como lista vacía si es null
                    Console.WriteLine("PlaylistCanciones estaba vacío, inicializando como lista vacía.");
                }

                // Obtener las canciones actuales asociadas con la playlist
                var currentSongs = playlist.PlaylistCanciones.Select(pc => pc.CancionCodigo).ToList();
                Console.WriteLine($"Canciones actuales en la playlist: {string.Join(", ", currentSongs)}");

                // Eliminar canciones que ya no están seleccionadas
                var songsToRemove = currentSongs.Except(Canciones).ToList();
                Console.WriteLine($"Canciones a eliminar: {string.Join(", ", songsToRemove)}");

                foreach (var songId in songsToRemove)
                {
                    var songToRemove = playlist.PlaylistCanciones.First(pc => pc.CancionCodigo == songId);
                    CRUD<PlaylistCancion>.Delete(songToRemove.Codigo); // Eliminar la relación de la base de datos
                    Console.WriteLine($"Canción con ID {songId} eliminada de la playlist.");
                }

                // Agregar canciones nuevas que no están en la lista actual
                var songsToAdd = Canciones.Except(currentSongs).ToList();
                Console.WriteLine($"Canciones a agregar: {string.Join(", ", songsToAdd)}");

                foreach (var songId in songsToAdd)
                {
                    var playlistCancion = new PlaylistCancion
                    {
                        PlaylistCodigo = playlist.Codigo,
                        CancionCodigo = songId
                    };
                    CRUD<PlaylistCancion>.Create(playlistCancion); // Crear la relación en la base de datos
                    Console.WriteLine($"Canción con ID {songId} agregada a la playlist.");
                }

                // Guardar los cambios
                Console.WriteLine("Guardando cambios...");
                // Aquí podrías llamar a tu servicio o lógica de persistencia para guardar los cambios

                // Redirigir a la página de detalles de la playlist
                Console.WriteLine("Redirigiendo a la página de detalles de la playlist.");
                return RedirectToAction(nameof(Details), new { id = playlist.Codigo });
            }
            catch (Exception ex)
            {
                // Capturar el error y mostrar un mensaje
                Console.WriteLine($"Error ocurrido: {ex.Message}");
                ViewBag.ErrorMessage = ex.Message;
                return View("Error"); // Redirigir a una página de error si falla
            }
        }






        // GET: PlayListsController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                // Obtener la playlist a eliminar usando el CRUD
                var playlist = CRUD<Playlist>.GetById(id);
                return View(playlist); // Mostrar la vista de confirmación de eliminación
            }
            catch (Exception ex)
            {
                // Si ocurre un error, mostrar mensaje
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }

        }

        // POST: PlayListsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Playlist playlist)
        {
            try
            {
                CRUD<Playlist>.Delete(id); // Eliminar una playlist usando el CRUD

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}