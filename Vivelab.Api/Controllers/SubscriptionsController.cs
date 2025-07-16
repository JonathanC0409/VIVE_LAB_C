using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vivelab.Modelos;

namespace Vivelab.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubscriptionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Suscripciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSuscripcion()
        {
            var susbcripciones = await _context.Subscriptions
                 .Include(s => s.PrimaryUser)
                 .Include(s => s.AdditionalUsers)
                 .Include(s => s.Plan)
                 .ToListAsync();

            return susbcripciones;
        }

        // GET: api/Suscripciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetSuscripcion(int id)
        {
            var suscripcion = await _context.Subscriptions
                .Where(s => s.Code == id)
                .Include(s => s.Plan)
                .Include(s => s.AdditionalUsers)
                .FirstAsync();

            if (suscripcion == null)
            {
                return NotFound();
            }

            return suscripcion;
        }

        // PUT: api/Suscripciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSuscripcion(int id, Subscription suscripcion)
        {
            if (id != suscripcion.Code)
            {
                return BadRequest();
            }

            _context.Entry(suscripcion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuscripcionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Suscripciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subscription>> PostSuscripcion(Subscription suscripcion)
        {
            _context.Subscriptions.Add(suscripcion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSuscripcion", new { id = suscripcion.Code }, suscripcion);
        }

        // DELETE: api/Suscripciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSuscripcion(int id)
        {
            var suscripcion = await _context.Subscriptions.FindAsync(id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(suscripcion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SuscripcionExists(int id)
        {
            return _context.Subscriptions.Any(e => e.Code == id);
        }
    }
}
