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
    public class PlaylistCancionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlaylistCancionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PlaylistCanciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistCancion>>> GetPlaylistCancion()
        {
            return await _context.PlaylistCanciones.ToListAsync();
        }

        // GET: api/PlaylistCanciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistCancion>> GetPlaylistCancion(int id)
        {
            var playlistCancion = await _context.PlaylistCanciones.FindAsync(id);

            if (playlistCancion == null)
            {
                return NotFound();
            }

            return playlistCancion;
        }

        // PUT: api/PlaylistCanciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaylistCancion(int id, PlaylistCancion playlistCancion)
        {
            if (id != playlistCancion.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(playlistCancion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaylistCancionExists(id))
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

        // POST: api/PlaylistCanciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlaylistCancion>> PostPlaylistCancion(PlaylistCancion playlistCancion)
        {
            _context.PlaylistCanciones.Add(playlistCancion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlaylistCancion", new { id = playlistCancion.Codigo }, playlistCancion);
        }

        // DELETE: api/PlaylistCanciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylistCancion(int id)
        {
            var playlistCancion = await _context.PlaylistCanciones.FindAsync(id);
            if (playlistCancion == null)
            {
                return NotFound();
            }

            _context.PlaylistCanciones.Remove(playlistCancion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlaylistCancionExists(int id)
        {
            return _context.PlaylistCanciones.Any(e => e.Codigo == id);
        }
    }
}
