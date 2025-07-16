using Microsoft.AspNetCore.Mvc;
using Vivelab.API.Consume;
using Vivelab.Modelos;

namespace Vivelab.Presentacion_MVC_.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult TopUpBalance()
        {
            int id = 0;
            foreach (var u in User.Claims)
            {
                if (u.Type == "UsuarioCodigo")
                {
                    id = int.Parse(u.Value);
                }
            }
            var usuario = CRUD<User>.GetById(id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        [HttpPost]
        public IActionResult TopUpBalance(int id, double monto)
        {

            var usuario = CRUD<User>.GetById(id);
            if (usuario == null) return NotFound();

            usuario.Balance += monto;
            CRUD<User>.Update(id, usuario);

            return RedirectToAction("Index", "Home");
        }
    }
}
