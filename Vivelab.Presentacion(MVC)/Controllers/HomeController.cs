using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Vivelab.API.Consume;
using Vivelab.Modelos;
using Vivelab.Presentacion_MVC_.Models;

namespace Vivelab.Presentacion_MVC_.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Artistas = getArtistas();
            ViewBag.Albumes = getAlbums();
            ViewBag.Canciones = getCanciones();
            ViewBag.TopCanciones = getTopCanciones();
            return View();
        }

        private List<User> getArtistas()
        {
            var usuarios = CRUD<User>.GetAll();
            var artistas = new List<User>();
            foreach (var u in usuarios)
            {
                if (u.Role == "artista")
                {
                    artistas.Add(u);
                }
            }
            return artistas;
        }

        private IEnumerable<Song> getTopCanciones()
        {
            // Ordena por total de reproducciones descendente y toma las 5 primeras
            return CRUD<Song>.GetAll()
                   .OrderByDescending(c => c.TotalPlays)
                   .Take(5)
                   .ToList();
        }

        private List<Album> getAlbums()
        {
            return CRUD<Album>.GetAll().ToList();
        }
        private List<Song> getCanciones()
        {
            return CRUD<Song>.GetAll().ToList();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Player(int id)
        {
            var cancion = CRUD<Song>.GetById(id);
            return View(cancion);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
