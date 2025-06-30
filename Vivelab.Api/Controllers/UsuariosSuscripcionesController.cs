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
    public class UsuariosSuscripcionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosSuscripcionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UsuariosSuscripciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioSuscripcion>>> GetUsuarioSuscripcion()
        {
            return await _context.UsuariosSuscripciones.ToListAsync();
        }

        // GET: api/UsuariosSuscripciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioSuscripcion>> GetUsuarioSuscripcion(int id)
        {
            var usuarioSuscripcion = await _context.UsuariosSuscripciones.FindAsync(id);

            if (usuarioSuscripcion == null)
            {
                return NotFound();
            }

            return usuarioSuscripcion;
        }

        // PUT: api/UsuariosSuscripciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarioSuscripcion(int id, UsuarioSuscripcion usuarioSuscripcion)
        {
            if (id != usuarioSuscripcion.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(usuarioSuscripcion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioSuscripcionExists(id))
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

        // POST: api/UsuariosSuscripciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuarioSuscripcion>> PostUsuarioSuscripcion(UsuarioSuscripcion usuarioSuscripcion)
        {
            _context.UsuariosSuscripciones.Add(usuarioSuscripcion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuarioSuscripcion", new { id = usuarioSuscripcion.Codigo }, usuarioSuscripcion);
        }

        // DELETE: api/UsuariosSuscripciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarioSuscripcion(int id)
        {
            var usuarioSuscripcion = await _context.UsuariosSuscripciones.FindAsync(id);
            if (usuarioSuscripcion == null)
            {
                return NotFound();
            }

            _context.UsuariosSuscripciones.Remove(usuarioSuscripcion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioSuscripcionExists(int id)
        {
            return _context.UsuariosSuscripciones.Any(e => e.Codigo == id);
        }
    }
}
