using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Vivelab.API.Consume;
using Vivelab.Modelos;

namespace Vivelab.Presentacion_MVC_.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: UsuarioController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult RecargarSaldo()
        {
            int id = 0;
            foreach (var u in User.Claims)
            {
                if (u.Type == "UsuarioCodigo")
                {
                    id = int.Parse(u.Value);
                }
            }
            var usuario = CRUD<Usuario>.GetById(id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        [HttpPost]
        public IActionResult RecargarSaldo(int id, double monto)
        {

            var usuario = CRUD<Usuario>.GetById(id);
            if (usuario == null) return NotFound();

            usuario.Saldo += monto;
            CRUD<Usuario>.Update(id, usuario);

            return RedirectToAction("Index", "Home");
        }

        // GET: UsuarioController/Usuarios
        public ActionResult ListaUsuarios()
        {
            // Obtener todos los usuarios
            var usuarios = CRUD<Usuario>.GetAll();

            // Pasar los usuarios a la vista
            return View(usuarios);
        }

        public ActionResult UsuarioBloqueado()
        {
            return View();
        }


        // GET: UsuarioController/BloquearUsuario
        public ActionResult BloquearUsuario()
        {
            var usuarios = CRUD<Usuario>.GetAll();

            // Pasar la lista de usuarios a la vista
            return View(usuarios);
        }


        // POST: UsuarioController/BloquearUsuario
        [HttpPost]
        public IActionResult BloquearUsuario(int usuarioId)
        {
            // Verificar si el usuario existe
            var usuario = CRUD<Usuario>.GetById(usuarioId);
            if (usuario == null)
            {
                return NotFound();
            }


            usuario.Rol = "bloqueado";
            CRUD<Usuario>.Update(usuarioId, usuario);

            // Redirigir a la lista de usuarios
            return RedirectToAction("Index", "Home");
        }

        public ActionResult VincularUsuario()
        {

            // Obtener los usuarios vinculados a la suscripción
            var usuariosVinculados = CRUD<UsuarioSuscripcion>.GetAll();
            ViewBag.UsuariosVinculados = usuariosVinculados;
            return View();
        }

        //Metodo vincular 
        [HttpPost]
        public async Task<IActionResult> VincularUsuario(string emailUsuarioVincular)
        {
            var emailUsuarioLogueado = "";
            var userId = 0;
            foreach (var user in User.Claims)
            {
                if (user.Type == ClaimTypes.Email)
                {
                    emailUsuarioLogueado = user.Value;
                }
                if (user.Type == "UsuarioCodigo")
                {
                    userId = int.Parse(user.Value);
                }

            }
            var usuario = CRUD<Usuario>.GetById(userId);

            if (usuario.Suscripcion == null)
            {
                ViewBag.ErrorMessage = "Usuario no tiene subcripcion";
                return View();
            }
            var SubId = usuario.Suscripcion.Codigo;
            var Subcripcion = CRUD<Suscripcion>.GetById(SubId);



            // Verificar si el correo del usuario a vincular no está vacío
            if (string.IsNullOrEmpty(emailUsuarioVincular))
            {
                ViewBag.ErrorMessage = "El correo del usuario a vincular no puede estar vacío.";
                return View();  // Regresar a la vista con el error
            }
            int cantidadPermitidad = usuario.Suscripcion.Plan.CantidadUsuarios;
            int cantidadUsuariosAdicionales = Subcripcion.UsuariosAdicionales?.Count() ?? 0;

            if (cantidadPermitidad > 0)
            {

                // Llamar al método del CRUD para vincular el usuario
                var resultado = await CRUD<Usuario>.VincularUsuarioASuscripcion(emailUsuarioVincular, emailUsuarioLogueado);

                if (resultado == "Usuario vinculado correctamente.")
                {
                    return RedirectToAction("Index", "Home");  // Redirigir a la página principal si todo es exitoso
                }

                else
                {
                    // Si hubo un error, mostrar el mensaje de error de la API
                    ViewBag.ErrorMessage = resultado;  // El mensaje de error de la API se pasa a ViewBag
                    return View();  // Volver a mostrar la vista con el mensaje de error
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Cantidad de usuarios superado";  // El mensaje de error de la API se pasa a ViewBag
                return View();
            }
        }


        // GET: UsuarioController/MisCanciones
        public ActionResult MisCanciones()
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
            var usuario = CRUD<Usuario>.GetById(id);
            if (usuario == null) return NotFound();

            // Suponiendo que Usuario tiene una lista de canciones
            var canciones = usuario.Canciones;
            return View(canciones); // Pasar las canciones a la vista
        }

        // GET: UsuarioController/EditarCancion/5
        public ActionResult EditarCancion(int id)
        {
            var cancion = CRUD<Cancion>.GetById(id); // Obtener la canción por su id
            if (cancion == null) return NotFound();

            return View(cancion); // Pasar la canción a la vista para editarla
        }

        // POST: UsuarioController/EditarCancion/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCancion(int id, Cancion cancionEditada)
        {
            try
            {
                // Obtener la canción original
                var cancion = CRUD<Cancion>.GetById(id);
                if (cancion == null) return NotFound();

                // Actualizar los valores de la canción
                cancion.Titulo = cancionEditada.Titulo;
                cancion.Duracion = cancionEditada.Duracion;

                // Guardar los cambios
                CRUD<Cancion>.Update(id, cancion);

                return RedirectToAction("MisCanciones"); // Redirigir al listado de canciones
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/EliminarCancion/5
        public ActionResult EliminarCancion(int id)
        {
            var cancion = CRUD<Cancion>.GetById(id); // Obtener la canción por su id
            if (cancion == null) return NotFound();

            return View(cancion); // Pasar la canción a la vista para confirmar su eliminación
        }

        // POST: UsuarioController/EliminarCancion/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarCancion(int id, IFormCollection collection)
        {
            try
            {
                var cancion = CRUD<Cancion>.GetById(id);
                if (cancion == null) return NotFound();

                CRUD<Cancion>.Delete(id); // Eliminar la canción

                return RedirectToAction("MisCanciones"); // Redirigir al listado de canciones
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/MisAlbumes
        public ActionResult MisAlbumes()
        {
            int id = 0;
            foreach (var u in User.Claims)
            {
                if (u.Type == "UsuarioCodigo")
                {
                    id = int.Parse(u.Value); // Obtener el id del usuario logueado
                }
            }

            // Obtener los álbumes del artista desde la base de datos
            var usuario = CRUD<Usuario>.GetById(id);
            if (usuario == null) return NotFound();

            // Suponiendo que Usuario tiene una lista de álbumes
            var albumes = usuario.Albums;
            return View(albumes); // Pasar los álbumes a la vista
        }

        // GET: UsuarioController/EditarAlbum/5
        public ActionResult EditarAlbum(int id)
        {
            var album = CRUD<Album>.GetById(id); // Obtener el álbum por su id
            if (album == null) return NotFound();

            return View(album); // Pasar el álbum a la vista para editarlo
        }

        // POST: UsuarioController/EditarAlbum/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarAlbum(int id, Album albumEditado)
        {
            try
            {
                // Obtener el álbum original
                var album = CRUD<Album>.GetById(id);
                if (album == null) return NotFound();

                // Actualizar los valores del álbum
                album.Nombre = albumEditado.Nombre;
                album.Canciones = albumEditado.Canciones;

                // Verificar si se ha cargado una nueva portada
                var archivo = Request.Form.Files["PortadaUrl"];
                if (archivo != null && archivo.Length > 0)
                {
                    // Generar un nombre único para la imagen
                    var fileName = Path.GetFileName(archivo.FileName);
                    var filePath = Path.Combine("wwwroot", "portadas", fileName);  // Ruta en el servidor para guardar la imagen

                    // Crear la carpeta si no existe
                    var directoryPath = Path.Combine("wwwroot", "portadas");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Guardar el archivo en el servidor
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        archivo.CopyToAsync(stream);
                    }

                    // Guardar la ruta relativa del archivo en la base de datos
                    album.PortadaUrl = "/portadas/" + fileName;
                }

                // Guardar los cambios en la base de datos
                CRUD<Album>.Update(id, album);

                return RedirectToAction("MisAlbumes"); // Redirigir al listado de álbumes
            }
            catch
            {
                return View();
            }
        }



        // GET: UsuarioController/EliminarAlbum/5
        public ActionResult EliminarAlbum(int id)
        {
            var album = CRUD<Album>.GetById(id); // Obtener el álbum por su id
            if (album == null) return NotFound();

            return View(album); // Pasar el álbum a la vista para confirmar su eliminación
        }

        // POST: UsuarioController/EliminarAlbum/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarAlbum(int id, Album album)
        {
            try
            {

                CRUD<Album>.Delete(id); // Eliminar el álbum

                return RedirectToAction("MisAlbumes"); // Redirigir al listado de álbumes
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/CrearAlbum
        public ActionResult CrearAlbum()
        {
            return View();
        }

        // POST: UsuarioController/CrearAlbum
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearAlbum(Album nuevoAlbum)
        {
            // Obtener el archivo de imagen cargado
            var archivo = Request.Form.Files["PortadaUrl"];

            if (ModelState.IsValid)
            {
                // Obtener el ID del usuario logueado
                int id = 0;
                foreach (var u in User.Claims)
                {
                    if (u.Type == "UsuarioCodigo")
                    {
                        id = int.Parse(u.Value); // Obtener el ID del usuario logueado
                    }
                }

                var usuario = CRUD<Usuario>.GetById(id);
                if (usuario == null) return NotFound();

                nuevoAlbum.ArtistaCodigo = usuario.Codigo; // Asignar el código del artista al nuevo álbum
                nuevoAlbum.FechaCreacion = DateTime.UtcNow; // Asignar la fecha de creación como la fecha actual

                // Si el archivo de imagen no es nulo, lo guardamos
                if (archivo != null && archivo.Length > 0)
                {
                    // Generar un nombre único para la imagen
                    var fileName = Path.GetFileName(archivo.FileName);
                    var filePath = Path.Combine("wwwroot", "portadas", fileName);  // Ruta en el servidor para guardar la imagen

                    // Crear la carpeta si no existe
                    var directoryPath = Path.Combine("wwwroot", "portadas");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Guardar el archivo en el servidor
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await archivo.CopyToAsync(stream);
                    }

                    // Guardar la ruta relativa del archivo en la base de datos (por ejemplo: "/portadas/imagen.jpg")
                    nuevoAlbum.PortadaUrl = "/portadas/" + fileName;
                }

                // Guardar el álbum
                CRUD<Album>.Create(nuevoAlbum);

                return RedirectToAction("MisAlbumes"); // Redirigir al listado de álbumes
            }

            return View(nuevoAlbum); // Si el modelo no es válido, retornar la vista con el error
        }


        public ActionResult VerCanciones(int albumId)
        {
            var album = CRUD<Album>.GetById(albumId);  // Obtener el álbum por ID
            if (album == null)
            {
                return NotFound();
            }

            // Si el álbum no tiene canciones, inicializar la lista
            if (album.Canciones == null)
            {
                album.Canciones = new List<Cancion>();  // Inicializar lista vacía si es null
            }

            // Obtener las canciones ya asociadas al álbum
            var canciones = album.Canciones;

            // Obtener las canciones disponibles que no están en el álbum
            var cancionesDisponibles = CRUD<Cancion>.GetAll().Where(c => !album.Canciones.Any(ac => ac.Codigo == c.Codigo)).ToList();

            ViewBag.AlbumId = albumId;  // Pasar el ID del álbum a la vista
            ViewBag.CancionesDisponibles = cancionesDisponibles;  // Pasar las canciones disponibles para agregar

            return View(canciones);  // Mostrar las canciones asociadas al álbum
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarCancion(int albumId, int cancionId)
        {
            var album = CRUD<Album>.GetById(albumId);  // Obtener el álbum
            var cancion = CRUD<Cancion>.GetById(cancionId);  // Obtener la canción seleccionada

            if (album == null || cancion == null)
            {
                return NotFound();
            }
            // Verificar si la canción ya está asociada a otro álbum
            if (cancion.AlbumCodigo != null)
            {
                TempData["ErrorMessage"] = "La canción ya está asociada a otro álbum.";  // Usamos TempData para almacenar el error
                return RedirectToAction("VerCanciones", new { albumId = albumId });  // Redirigir a la vista de canciones del álbum
            }
            // Asociar la canción con el álbum
            cancion.AlbumCodigo = albumId;

            CRUD<Cancion>.Update(cancionId, cancion);  // Actualizar el álbum con la nueva canción

            return RedirectToAction("VerCanciones", new { albumId = albumId });  // Redirigir a la vista de canciones del álbum
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarCancionDelAlbum(int albumId, int cancionId)
        {
            var album = CRUD<Album>.GetById(albumId);  // Obtener el álbum
            var cancion = CRUD<Cancion>.GetById(cancionId);  // Obtener la canción seleccionada

            if (album == null || cancion == null)
            {
                return NotFound();  // Si el álbum o la canción no existen, devolver 404
            }

            cancion.AlbumCodigo = null;
            CRUD<Cancion>.Update(cancionId, cancion);
            return RedirectToAction("VerCanciones", new { albumId = albumId });



        }


    }
}
