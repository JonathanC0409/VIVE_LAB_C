using Microsoft.AspNetCore.Mvc;
using Vivelab.API.Consume;
using Vivelab.Modelos;
using Vivelab.Presentacion_MVC_.Models;

namespace Vivelab.Presentacion_MVC_.Controllers
{
    public class MusicaController : Controller
    {
        public IActionResult Index()
        {
            var lista = CRUD<Cancion>.GetAll();
            return View(lista);
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

    }
}
