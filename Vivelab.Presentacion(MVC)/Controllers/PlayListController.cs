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
            var usuarioCodigo = 0;
            foreach (var u in User.Claims)
            {
                {
                    usuarioCodigo = int.Parse(u.Value);
                }
            }
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
        public ActionResult Edit(int id)
        {
            try
            {
                // Obtener la playlist a editar usando el CRUD
                var playlist = CRUD<Playlist>.GetById(id);
                return View(playlist); // Mostrar la vista de edición con los datos de la playlist
            }
            catch (Exception ex)
            {
                // Si ocurre un error, mostrar mensaje
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }

        }

        // POST: PlayListsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Playlist playlist)
        {
            try
            {
                CRUD<Playlist>.Update(id, playlist); // Actualizar una playlist existente usando el CRUD
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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