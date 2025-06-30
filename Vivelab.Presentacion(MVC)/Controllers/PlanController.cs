using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vivelab.API.Consume;
using Vivelab.Modelos;
using Vivelab.Servicios.Interfaces;

namespace Vivelab.Presentacion_MVC_.Controllers
{

    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        private readonly IMetodoPagoService _metodoPagoService;
        private readonly ISuscripcionService _suscripcionService;

        public PlanController(IPlanService planService, IMetodoPagoService metodoPagoService, ISuscripcionService suscripcionService)
        {
            _planService = planService;
            _metodoPagoService = metodoPagoService;
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

        // POST: Plan/Comprar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Comprar(int planCodigo, string tokenPago)
        {
            int usuarioId = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst("UsuarioCodigo").Value) : 0;

            // 2) Valida plan
            if (!await _planService.ElejirPlan(planCodigo))
                return BadRequest("Plan no encontrado.");

            // 3) Registra método de pago (PeiGo)
            var metodo = new MetodoPago
            {
                PeiGoPaymentToken = tokenPago,
                FechaRegistro = DateTime.Now
            };
            var creado = CRUD<MetodoPago>.Create(metodo);
            if (creado == null)
                return BadRequest("Error al registrar el pago.");

            if (!await _metodoPagoService.ElejirMetodoPago(creado.Codigo))
                return BadRequest("Método de pago inválido.");

            // 4) Crea la suscripción
            var suscripcion = new Suscripcion
            {
                PlanCodigo = planCodigo,
                MetodoPagoCodigo = creado.Codigo,
                UsuarioCodigo = usuarioId,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddMonths(1),
                Estado = "Activa"
            };
            var ok = await _suscripcionService.CrearSuscripcion(suscripcion);
            if (!ok)
                return BadRequest("No se pudo crear la suscripción.");

            // 5) Redirige de vuelta al listado (o a una página de "Gracias")
            return RedirectToAction(nameof(Index));
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
    }
}
