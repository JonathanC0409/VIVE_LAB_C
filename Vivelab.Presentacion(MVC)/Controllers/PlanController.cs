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
            var usuario = CRUD<User>.GetAll().FirstOrDefault(u => u.Email == email);
            var plan = CRUD<Plan>.GetAll().FirstOrDefault(p => p.Code == id);

            if (usuario == null || plan == null)
            {
                TempData["Mensaje"] = "No se pudo completar la compra. Datos inválidos.";
                return RedirectToAction("Index", "Plan");
            }

            // Verificar si el usuario tiene saldo suficiente antes de continuar
            if (usuario.Balance < plan.Price)
            {
                TempData["Mensaje"] = "Saldo insuficiente para realizar la compra.";
                return RedirectToAction("Index", "Plan");
            }

            // Verificar si el usuario ya tiene una suscripción activa
            var suscripcionActiva = CRUD<Subscription>.GetAll()
                .FirstOrDefault(s => s.UserCode == usuario.Code && s.Status == "Activa");

            if (suscripcionActiva != null)
            {
                var planActivo = CRUD<Plan>.GetAll().FirstOrDefault(p => p.Code == suscripcionActiva.PlanCode);

                if (planActivo == null)
                {
                    TempData["Mensaje"] = "No se pudo encontrar el plan asociado a la suscripción activa.";
                    return RedirectToAction("Index", "Plan");
                }

                // Verifica si el usuario está intentando cambiar de un plan más barato a uno más caro
                if (planActivo.Name == "Free" && (plan.Name == "Premium" || plan.Name == "Familiar" || plan.Name == "Empresarial"))
                {
                    // Verificar si tiene saldo suficiente para el nuevo plan
                    if (usuario.Balance < plan.Price)
                    {
                        TempData["Mensaje"] = "Saldo insuficiente para realizar el cambio al nuevo plan.";
                        return RedirectToAction("Index", "Plan");
                    }

                    // Actualiza la suscripción al nuevo plan y establece la fecha de fin a 1 mes
                    suscripcionActiva.PlanCode = plan.Code;
                    suscripcionActiva.EndDate = DateTime.UtcNow.AddMonths(1); // El cambio se hará efectivo durante 1 mes
                    CRUD<Subscription>.Update(suscripcionActiva.Code, suscripcionActiva);

                    // Descontar saldo
                    usuario.Balance -= plan.Price;
                    CRUD<User>.Update(usuario.Code, usuario);

                    TempData["Mensaje"] = "Cambio de plan realizado exitosamente. El cambio se hará efectivo durante 1 mes.";
                }
                else if (planActivo.Name != "Free")
                {
                    if (plan.Name == "Free")
                    {
                        TempData["Mensaje"] = "Actualmente tienes el plan " + planActivo.Name + ". El cambio a plan Free se hará cuando se termine tu suscripción actual.";
                        return RedirectToAction("Index", "Plan");
                    }

                    // Verificar si tiene saldo suficiente para el nuevo plan
                    if (usuario.Balance < plan.Price)
                    {
                        TempData["Mensaje"] = "Saldo insuficiente para realizar el cambio al nuevo plan.";
                        return RedirectToAction("Index", "Plan");
                    }

                    // Si el plan actual no es Free, podemos hacer el cambio entre planes Premium, Familiar o Empresarial
                    if (planActivo.Name != plan.Name)
                    {
                        suscripcionActiva.PlanCode = plan.Code;
                        suscripcionActiva.EndDate = DateTime.UtcNow.AddMonths(1); // Actualiza la fecha de fin a 1 mes
                        CRUD<Subscription>.Update(suscripcionActiva.Code, suscripcionActiva);

                        // Descontar saldo
                        usuario.Balance -= plan.Price;
                        CRUD<User>.Update(usuario.Code, usuario);

                        TempData["Mensaje"] = "Cambio de plan realizado exitosamente. El cambio se hará efectivo durante 1 mes.";
                    }
                    else
                    {
                        TempData["Mensaje"] = $"Ya tienes el plan {planActivo.Name}.";
                        return RedirectToAction("Index", "Plan");
                    }
                }
            }
            else
            {
                // Si el usuario no tiene suscripción activa, se puede crear una nueva
                if (usuario.Balance >= plan.Price)
                {
                    usuario.Balance -= plan.Price;
                    CRUD<User>.Update(usuario.Code, usuario);

                    var nuevaSuscripcion = new Subscription
                    {
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddMonths(1),
                        Status = "Activa",
                        PlanCode = plan.Code,
                        UserCode = usuario.Code
                    };

                    CRUD<Subscription>.Create(nuevaSuscripcion);

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
