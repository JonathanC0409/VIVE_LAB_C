using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vivelab.API.Consume;
using Vivelab.Modelos;
using Vivelab.Servicios.Interfaces;

namespace Vivelab.Presentacion_MVC_.Controllers
{

    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        private readonly ISuscripcionService _suscripcionService;

        public PlanController(IPlanService planService, ISuscripcionService suscripcionService)
        {
            _planService = planService;

            _suscripcionService = suscripcionService;
        }

        // GET: PlanController
        public ActionResult Index()
        {

            var planes = CRUD<Plan>.GetAll();
            return View(planes);

        }

        // GET: Plan/Comprar/5
        public ActionResult Comprar(int id)
        {
            var plan = CRUD<Plan>.GetById(id);
            if (plan == null) return NotFound();
            return View(plan);
        }


        // GET: PlanController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlanController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlanController/Create
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

        // GET: PlanController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlanController/Edit/5
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

        // GET: PlanController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlanController/Delete/5
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
        [Authorize]
        [Authorize]
        public IActionResult ComprarPlan(int id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var usuario = CRUD<Usuario>.GetAll().FirstOrDefault(u => u.Email == email);
            var plan = CRUD<Plan>.GetAll().FirstOrDefault(p => p.Codigo == id);

            if (usuario == null || plan == null)
            {
                TempData["Mensaje"] = "No se pudo completar la compra. Datos inválidos.";
                return RedirectToAction("Index", "Plan");
            }

            // Verificar si el usuario tiene saldo suficiente antes de continuar
            if (usuario.Saldo < plan.Precio)
            {
                TempData["Mensaje"] = "Saldo insuficiente para realizar la compra.";
                return RedirectToAction("Index", "Plan");
            }

            // Verificar si el usuario ya tiene una suscripción activa
            var suscripcionActiva = CRUD<Suscripcion>.GetAll()
                .FirstOrDefault(s => s.UsuarioCodigo == usuario.Codigo && s.Estado == "Activa");

            if (suscripcionActiva != null)
            {
                var planActivo = CRUD<Plan>.GetAll().FirstOrDefault(p => p.Codigo == suscripcionActiva.PlanCodigo);

                if (planActivo == null)
                {
                    TempData["Mensaje"] = "No se pudo encontrar el plan asociado a la suscripción activa.";
                    return RedirectToAction("Index", "Plan");
                }

                // Verifica si el usuario está intentando cambiar de un plan más barato a uno más caro
                if (planActivo.Nombre == "Free" && (plan.Nombre == "Premium" || plan.Nombre == "Familiar" || plan.Nombre == "Empresarial"))
                {
                    // Verificar si tiene saldo suficiente para el nuevo plan
                    if (usuario.Saldo < plan.Precio)
                    {
                        TempData["Mensaje"] = "Saldo insuficiente para realizar el cambio al nuevo plan.";
                        return RedirectToAction("Index", "Plan");
                    }

                    // Actualiza la suscripción al nuevo plan y establece la fecha de fin a 1 mes
                    suscripcionActiva.PlanCodigo = plan.Codigo;
                    suscripcionActiva.FechaFin = DateTime.UtcNow.AddMonths(1); // El cambio se hará efectivo durante 1 mes
                    CRUD<Suscripcion>.Update(suscripcionActiva.Codigo, suscripcionActiva);

                    // Descontar saldo
                    usuario.Saldo -= plan.Precio;
                    CRUD<Usuario>.Update(usuario.Codigo, usuario);

                    TempData["Mensaje"] = "Cambio de plan realizado exitosamente. El cambio se hará efectivo durante 1 mes.";
                }
                else if (planActivo.Nombre != "Free")
                {
                    if (plan.Nombre == "Free")
                    {
                        TempData["Mensaje"] = "Actualmente tienes el plan " + planActivo.Nombre + ". El cambio a plan Free se hará cuando se termine tu suscripción actual.";
                        return RedirectToAction("Index", "Plan");
                    }

                    // Verificar si tiene saldo suficiente para el nuevo plan
                    if (usuario.Saldo < plan.Precio)
                    {
                        TempData["Mensaje"] = "Saldo insuficiente para realizar el cambio al nuevo plan.";
                        return RedirectToAction("Index", "Plan");
                    }

                    // Si el plan actual no es Free, podemos hacer el cambio entre planes Premium, Familiar o Empresarial
                    if (planActivo.Nombre != plan.Nombre)
                    {
                        suscripcionActiva.PlanCodigo = plan.Codigo;
                        suscripcionActiva.FechaFin = DateTime.UtcNow.AddMonths(1); // Actualiza la fecha de fin a 1 mes
                        CRUD<Suscripcion>.Update(suscripcionActiva.Codigo, suscripcionActiva);

                        // Descontar saldo
                        usuario.Saldo -= plan.Precio;
                        CRUD<Usuario>.Update(usuario.Codigo, usuario);

                        TempData["Mensaje"] = "Cambio de plan realizado exitosamente. El cambio se hará efectivo durante 1 mes.";
                    }
                    else
                    {
                        TempData["Mensaje"] = $"Ya tienes el plan {planActivo.Nombre}.";
                        return RedirectToAction("Index", "Plan");
                    }
                }
            }
            else
            {
                // Si el usuario no tiene suscripción activa, se puede crear una nueva
                if (usuario.Saldo >= plan.Precio)
                {
                    usuario.Saldo -= plan.Precio;
                    CRUD<Usuario>.Update(usuario.Codigo, usuario);

                    var nuevaSuscripcion = new Suscripcion
                    {
                        FechaInicio = DateTime.UtcNow,
                        FechaFin = DateTime.UtcNow.AddMonths(1),
                        Estado = "Activa",
                        PlanCodigo = plan.Codigo,
                        UsuarioCodigo = usuario.Codigo
                    };

                    CRUD<Suscripcion>.Create(nuevaSuscripcion);

                    TempData["Mensaje"] = "Compra realizada exitosamente.";
                }
                else
                {
                    TempData["Mensaje"] = "Saldo insuficiente para realizar la compra.";
                }
            }

            return RedirectToAction("Index", "Plan");
        }



    }
}
