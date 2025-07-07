using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            CRUD<Usuario>.Update(id, usuario); // o el método que estés usando para guardar

            return RedirectToAction("Index", "Home"); // o a donde quieras volver
        }

        // GET: UsuarioController/Usuarios
        public ActionResult ListaUsuarios()
        {
            // Obtener todos los usuarios
            var usuarios = CRUD<Usuario>.GetAll(); // Asegúrate de que GetAll() obtenga todos los usuarios de la base de datos

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
            // Obtener todos los usuarios (o aplicar filtros según tu necesidad)
            var usuarios = CRUD<Usuario>.GetAll(); // Asegúrate de que GetAll() obtenga todos los usuarios de la base de datos

            // Pasar la lista de usuarios a la vista
            return View(usuarios);
        }


        // POST: UsuarioController/BloquearUsuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BloquearUsuario(int usuarioId)
        {
            // Verificar si el usuario existe
            var usuario = CRUD<Usuario>.GetById(usuarioId);
            if (usuario == null)
            {
                return NotFound();  // Si el usuario no se encuentra, devuelve un error 404
            }

            // Marcar al usuario como bloqueado
            usuario.TipoUsuario = "bloqueado";  // Asegúrate de que el modelo Usuario tenga esta propiedad
            CRUD<Usuario>.Update(usuarioId, usuario); // Actualizar el usuario en la base de datos

            // Redirigir a la lista de usuarios
            return RedirectToAction("ListaUsuarios", "Usuario");
        }





    }
}
