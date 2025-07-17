using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        //// GET: Plan/Comprar/5
        //public ActionResult Buy(int id)
        //{
        //    var plan = CRUD<Plan>.GetById(id);
        //    if (plan == null) return NotFound();
        //    return View(plan);
        //}
        //[Authorize]
        //[Authorize]
        //public IActionResult BuyPlan(int id)
        //{
        //    var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        //    var usuario = CRUD<User>.GetAll().FirstOrDefault(u => u.Email == email);
        //    var plan = CRUD<Plan>.GetAll().FirstOrDefault(p => p.Code == id);

        //    if (usuario == null || plan == null)
        //    {
        //        TempData["Mensaje"] = "No se pudo completar la compra. Datos inválidos.";
        //        return RedirectToAction("BuyPlan", "Client");
        //    }

        //    // Verificar si el usuario tiene saldo suficiente antes de continuar
        //    if (usuario.Balance < plan.Price)
        //    {
        //        TempData["Mensaje"] = "Saldo insuficiente para realizar la compra.";
        //        return RedirectToAction("BuyPlan", "Client");
        //    }

        //    // Verificar si el usuario ya tiene una suscripción activa
        //    var suscripcionActiva = CRUD<Subscription>.GetAll()
        //        .FirstOrDefault(s => s.UserCode == usuario.Code && s.Status == "Activa");

        //    if (suscripcionActiva != null)
        //    {
        //        var planActivo = CRUD<Plan>.GetAll().FirstOrDefault(p => p.Code == suscripcionActiva.PlanCode);

        //        if (planActivo == null)
        //        {
        //            TempData["Mensaje"] = "No se pudo encontrar el plan asociado a la suscripción activa.";
        //            return RedirectToAction("BuyPlan", "Client");
        //        }

        //        // Verifica si el usuario está intentando cambiar de un plan más barato a uno más caro
        //        if (planActivo.Name == "Free" && (plan.Name == "Premium" || plan.Name == "Familiar" || plan.Name == "Empresarial"))
        //        {
        //            // Verificar si tiene saldo suficiente para el nuevo plan
        //            if (usuario.Balance < plan.Price)
        //            {
        //                TempData["Mensaje"] = "Saldo insuficiente para realizar el cambio al nuevo plan.";
        //                return RedirectToAction("BuyPlan", "Client");
        //            }

        //            // Actualiza la suscripción al nuevo plan y establece la fecha de fin a 1 mes
        //            suscripcionActiva.PlanCode = plan.Code;
        //            suscripcionActiva.EndDate = DateTime.UtcNow.AddMonths(1); // El cambio se hará efectivo durante 1 mes
        //            CRUD<Subscription>.Update(suscripcionActiva.Code, suscripcionActiva);

        //            // Descontar saldo
        //            usuario.Balance -= plan.Price;
        //            CRUD<User>.Update(usuario.Code, usuario);

        //            TempData["Mensaje"] = "Cambio de plan realizado exitosamente. El cambio se hará efectivo durante 1 mes.";
        //        }
        //        else if (planActivo.Name != "Free")
        //        {
        //            if (plan.Name == "Free")
        //            {
        //                TempData["Mensaje"] = "Actualmente tienes el plan " + planActivo.Name + ". El cambio a plan Free se hará cuando se termine tu suscripción actual.";
        //                return RedirectToAction("BuyPlan", "Client");
        //            }

        //            // Verificar si tiene saldo suficiente para el nuevo plan
        //            if (usuario.Balance < plan.Price)
        //            {
        //                TempData["Mensaje"] = "Saldo insuficiente para realizar el cambio al nuevo plan.";
        //                return RedirectToAction("BuyPlan", "Client");
        //            }

        //            // Si el plan actual no es Free, podemos hacer el cambio entre planes Premium, Familiar o Empresarial
        //            if (planActivo.Name != plan.Name)
        //            {
        //                suscripcionActiva.PlanCode = plan.Code;
        //                suscripcionActiva.EndDate = DateTime.UtcNow.AddMonths(1); // Actualiza la fecha de fin a 1 mes
        //                CRUD<Subscription>.Update(suscripcionActiva.Code, suscripcionActiva);

        //                // Descontar saldo
        //                usuario.Balance -= plan.Price;
        //                CRUD<User>.Update(usuario.Code, usuario);

        //                TempData["Mensaje"] = "Cambio de plan realizado exitosamente. El cambio se hará efectivo durante 1 mes.";
        //            }
        //            else
        //            {
        //                TempData["Mensaje"] = $"Ya tienes el plan {planActivo.Name}.";
        //                return RedirectToAction("BuyPlan", "Client");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // Si el usuario no tiene suscripción activa, se puede crear una nueva
        //        if (usuario.Balance >= plan.Price)
        //        {
        //            usuario.Balance -= plan.Price;
        //            CRUD<User>.Update(usuario.Code, usuario);

        //            var nuevaSuscripcion = new Subscription
        //            {
        //                StartDate = DateTime.UtcNow,
        //                EndDate = DateTime.UtcNow.AddMonths(1),
        //                Status = "Activa",
        //                PlanCode = plan.Code,
        //                UserCode = usuario.Code
        //            };

        //            CRUD<Subscription>.Create(nuevaSuscripcion);

        //            TempData["Mensaje"] = "Compra realizada exitosamente.";
        //        }
        //        else
        //        {
        //            TempData["Mensaje"] = "Saldo insuficiente para realizar la compra.";
        //        }
        //    }

        //    return RedirectToAction("BuyPlan", "Client");
        //}


        public ActionResult LinkUsers()
        {

            // Obtener los usuarios vinculados a la suscripción
            var usuariosVinculados = CRUD<UserSubscription>.GetAll();
            ViewBag.UsuariosVinculados = usuariosVinculados;
            return View();
        }

        //Metodo vincular 
        [HttpPost]
        public async Task<IActionResult> LinkUsers(string emailUsuarioVincular)
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
            var usuario = CRUD<User>.GetById(userId);

            if (usuario.Subscription == null)
            {
                ViewBag.ErrorMessage = "Usuario no tiene subcripcion";
                return View();
            }
            var SubId = usuario.Subscription.Code;
            var Subcripcion = CRUD<Subscription>.GetById(SubId);



            // Verificar si el correo del usuario a vincular no está vacío
            if (string.IsNullOrEmpty(emailUsuarioVincular))
            {
                ViewBag.ErrorMessage = "El correo del usuario a vincular no puede estar vacío.";
                return View();  // Regresar a la vista con el error
            }
            int cantidadPermitidad = usuario.Subscription.Plan.UserCount;
            int cantidadUsuariosAdicionales = Subcripcion.AdditionalUsers?.Count() ?? 0;

            if (cantidadPermitidad > 0)
            {

                // Llamar al método del CRUD para vincular el usuario
                var resultado = await CRUD<User>.VincularUsuarioASuscripcion(emailUsuarioVincular, emailUsuarioLogueado);

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

    }
}
