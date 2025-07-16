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
    public class PlaylistSongsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlaylistSongsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PlaylistCanciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistSong>>> GetPlaylistCancion()
        {
            return await _context.PlaylistSongs.ToListAsync();
        }

        // GET: api/PlaylistCanciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistSong>> GetPlaylistCancion(int id)
        {
            var playlistCancion = await _context.PlaylistSongs.FindAsync(id);

            if (playlistCancion == null)
            {
                return NotFound();
            }

            return playlistCancion;
        }

        // PUT: api/PlaylistCanciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaylistCancion(int id, PlaylistSong playlistCancion)
        {
            if (id != playlistCancion.Code)
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
        public async Task<ActionResult<PlaylistSong>> PostPlaylistCancion(PlaylistSong playlistCancion)
        {
            _context.PlaylistSongs.Add(playlistCancion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlaylistCancion", new { id = playlistCancion.Code }, playlistCancion);
        }

        // DELETE: api/PlaylistCanciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylistCancion(int id)
        {
            var playlistCancion = await _context.PlaylistSongs.FindAsync(id);
            if (playlistCancion == null)
            {
                return NotFound();
            }

            _context.PlaylistSongs.Remove(playlistCancion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlaylistCancionExists(int id)
        {
            return _context.PlaylistSongs.Any(e => e.Code == id);
        }
        // GET: api/PlaylistCanciones/playlist/5
        [HttpGet("playlist/{id}")]
        public async Task<ActionResult<IEnumerable<PlaylistSong>>> GetPlaylistCancionesByPlaylistId(int id)
        {
            // Buscar las canciones asociadas a la playlist
            var playlistCanciones = await _context.PlaylistSongs
                .Where(pc => pc.PlaylistCode == id)
                .Include(pc => pc.Song) // Incluir la canción asociada
                .ToListAsync();

            if (playlistCanciones == null || playlistCanciones.Count == 0)
            {
                return NotFound(); // Si no hay canciones asociadas, devolver un 404
            }

            return playlistCanciones; // Devolver la lista de PlaylistCancion
        }
    }
}
