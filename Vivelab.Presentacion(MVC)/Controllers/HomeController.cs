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

        private List<Usuario> getArtistas()
        {
            var usuarios = CRUD<Usuario>.GetAll();
            var artistas = new List<Usuario>();
            foreach (var u in usuarios)
            {
                if(u.Rol == "artista")
                {
                    artistas.Add(u);
                }
            }
            return artistas;
        }

        private IEnumerable<Cancion> getTopCanciones()
        {
            // Ordena por total de reproducciones descendente y toma las 5 primeras
            return CRUD<Cancion>.GetAll()
                   .OrderByDescending(c => c.TotalReproducciones)
                   .Take(5)
                   .ToList();
        }

        private List<Album> getAlbums()
        {
            return CRUD<Album>.GetAll().ToList();
        }
        private List<Cancion> getCanciones()
        {
            return CRUD<Cancion>.GetAll().ToList();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Player(int id)
        {
            var cancion = CRUD<Cancion>.GetById(id);
            return View(cancion);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
