using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
                var playlists = CRUD<Playlist>.GetBy("usuario", usuarioCodigo);
                return View(playlists); // Mostrar todas las playlists
            }
            catch (Exception ex)
            {
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

            // Obtener el usuario principal (logueado)
            var usuario = CRUD<User>.GetById(UsuarioId);
            if (usuario != null)
            {
                // Si tiene una suscripción activa, se retorna el plan
                if (usuario.Subscription != null)
                {
                    return usuario.Subscription.Plan.Code;
                }

                // Si no tiene una suscripción activa, se busca en las suscripciones de otros usuarios si lo tienen vinculado
                var usuarioSubcripciones = CRUD<UserSubscription>.GetAll();
                foreach (var u in usuarioSubcripciones)
                {
                    if (usuario.Code == u.UserCode)
                    {
                        return u.Subscription.PlanCode;
                    }
                }

            }
            return 0;
        }

        // GET: PlayListsController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                Console.WriteLine($"Inicio del proceso para obtener los detalles de la playlist con ID: {id}");

                var playlist = CRUD<Playlist>.GetById(id);

                if (playlist == null)
                {
                    Console.WriteLine($"Playlist con ID {id} no encontrada.");
                    return NotFound();
                }

                Console.WriteLine($"Playlist obtenida: {playlist.Name}");

                // Hacer una llamada a la API para obtener las canciones asociadas a esta playlist
                string apiUrl = $"https://localhost:7008/api/PlaylistCanciones/playlist/{id}";
                var playlistCanciones = new List<PlaylistSong>();

                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        playlistCanciones = JsonConvert.DeserializeObject<List<PlaylistSong>>(json);
                        Console.WriteLine($"Canciones encontradas en la playlist {playlist.Name}:");

                        foreach (var playlistCancion in playlistCanciones)
                        {
                            Console.WriteLine($"Canción ID: {playlistCancion.SongCode} - Nombre: {playlistCancion.Song.Title}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error al obtener canciones de la API. Estado: {response.StatusCode}");
                    }
                }

                ViewBag.PlaylistCanciones = playlistCanciones;

                return View(playlist);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ocurrido: {ex.Message}");
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: PlayListsController/ConfirmDelete/{playlistId}/{songId}
        public ActionResult ConfirmDelete(int playlistId, int songId)
        {
            try
            {
                var playlist = CRUD<Playlist>.GetById(playlistId);
                var cancion = CRUD<Song>.GetById(songId);

                if (playlist == null || cancion == null)
                {
                    return NotFound();
                }

                ViewBag.Playlist = playlist;
                ViewBag.Cancion = cancion;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // POST: PlayListsController/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int playlistId, int songId)
        {
            try
            {
                var playlistCancion = CRUD<PlaylistSong>.GetBy("playlist", playlistId)
                                                          .FirstOrDefault(pc => pc.SongCode == songId);

                if (playlistCancion != null)
                {
                    CRUD<PlaylistSong>.Delete(playlistCancion.Code);
                    Console.WriteLine($"Canción con ID {songId} eliminada de la playlist {playlistId}.");
                }

                return RedirectToAction(nameof(Details), new { id = playlistId });
            }
            catch (Exception ex)
            {
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
                CRUD<Playlist>.Create(playlist);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlayListsController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var playlist = CRUD<Playlist>.GetById(id);

                if (playlist == null)
                {
                    return NotFound();
                }

                var allSongs = CRUD<Song>.GetAll();
                ViewBag.AllSongs = allSongs;

                return View(playlist);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // POST: PlayListsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, List<int> Canciones)
        {
            try
            {
                // Obtener la playlist a editar usando el CRUD
                var playlist = CRUD<Playlist>.GetById(id);

                if (playlist == null)
                {
                    return NotFound(); // Si la playlist no existe
                }

                // Verificar que PlaylistCanciones no sea null
                if (playlist.PlaylistSongs == null)
                {
                    playlist.PlaylistSongs = new List<PlaylistSong>(); // Inicializar como lista vacía si es null
                }

                // Obtener las canciones actuales asociadas con la playlist
                var currentSongs = playlist.PlaylistSongs.Select(pc => pc.SongCode).ToList();

                // Comprobar si alguna de las canciones seleccionadas ya está en la playlist
                foreach (var songId in Canciones)
                {

                    // Verificamos si la canción ya está en la lista actual
                    if (currentSongs.Contains(songId))
                    {
                        TempData["Mensaje"] = $"La canción con ID {songId} ya está agregada a esta playlist.";

                        // Redirigir a la vista de detalles de la playlist con el mensaje de duplicado
                        return RedirectToAction(nameof(Details), new { id = playlist.Code });
                    }
                }

                // Agregar canciones nuevas que no están en la lista actual
                var songsToAdd = Canciones.Except(currentSongs).ToList();
                foreach (var songId in songsToAdd)
                {
                    var playlistCancion = new PlaylistSong
                    {
                        PlaylistCode = playlist.Code,
                        SongCode = songId
                    };
                    CRUD<PlaylistSong>.Create(playlistCancion); // Crear la relación en la base de datos
                }

                // Redirigir al detalle de la playlist después de realizar cambios
                return RedirectToAction(nameof(Details), new { id = playlist.Code });
            }
            catch (Exception ex)
            {
                // Capturar el error y mostrar un mensaje
                Console.WriteLine($"Error ocurrido: {ex.Message}");
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }


        // GET: PlayListsController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var playlist = CRUD<Playlist>.GetById(id);
                return View(playlist);
            }
            catch (Exception ex)
            {
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
                CRUD<Playlist>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}