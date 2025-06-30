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
    public class MetodoPagosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MetodoPagosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MetodoPagos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetodoPago>>> GetMetodoPago()
        {
            return await _context.MetodoPagos.ToListAsync();
        }

        // GET: api/MetodoPagos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MetodoPago>> GetMetodoPago(int id)
        {
            var metodoPago = await _context.MetodoPagos.FindAsync(id);

            if (metodoPago == null)
            {
                return NotFound();
            }

            return metodoPago;
        }

        // PUT: api/MetodoPagos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMetodoPago(int id, MetodoPago metodoPago)
        {
            if (id != metodoPago.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(metodoPago).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MetodoPagoExists(id))
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

        // POST: api/MetodoPagos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MetodoPago>> PostMetodoPago(MetodoPago metodoPago)
        {
            _context.MetodoPagos.Add(metodoPago);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMetodoPago", new { id = metodoPago.Codigo }, metodoPago);
        }

        // DELETE: api/MetodoPagos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMetodoPago(int id)
        {
            var metodoPago = await _context.MetodoPagos.FindAsync(id);
            if (metodoPago == null)
            {
                return NotFound();
            }

            _context.MetodoPagos.Remove(metodoPago);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MetodoPagoExists(int id)
        {
            return _context.MetodoPagos.Any(e => e.Codigo == id);
        }
    }
}
