using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Vivelab.Modelos;

namespace Vivelab.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.UsuariosSuscripciones) // Incluir la relación con UsuariosSuscripciones
                .Include(u => u.Suscripcion) // Incluir la suscripción del usuario (si es el propietario)
                .Include(u => u.Suscripcion.Plan) // Incluir el plan de la suscripción
                .ToListAsync();

            return usuarios;

        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Codigo == id)
                .Include(u => u.Suscripcion)
                .ThenInclude(u => u.Plan)
                .FirstAsync();

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Codigo)
            {
                return BadRequest();
            }

            // Obtener el usuario actual de la base de datos
            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound();
            }

            // Verificar si la contraseña proporcionada es diferente a la almacenada
            // y si NO es un hash BCrypt válido (es decir, es texto plano)
            if (usuario.Password != usuarioExistente.Password &&
                !BCrypt.Net.BCrypt.Verify(usuario.Password, usuarioExistente.Password))
            {
                // Solo hashear si la contraseña es nueva (texto plano)
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            }
            else
            {
                // Si ya está hasheada, mantener el valor existente
                usuario.Password = usuarioExistente.Password;
            }

            _context.Entry(usuarioExistente).CurrentValues.SetValues(usuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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
        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Codigo }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Codigo == id);
        }
        [HttpGet("VincularUsuarios")]
        public async Task<IActionResult> VincularUsuarios(string email, string emailLogeado)
        {
            if (email == emailLogeado)
            {
                return BadRequest("No puedes vincularte a ti mismo.");
            }

            // Verificar si el correo del usuario a vincular es válido
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("El correo del usuario a vincular no puede estar vacío.");
            }

            // Obtener el correo del usuario logueado y verificar su suscripción activa
            var usuarioLogueado = await _context.Usuarios
                .Include(u => u.Suscripcion) // Incluir suscripción
                .Include(u => u.UsuariosSuscripciones)
                .FirstOrDefaultAsync(u => u.Email == emailLogeado);

            if (usuarioLogueado == null || usuarioLogueado.Suscripcion == null || usuarioLogueado.Suscripcion.FechaFin <= DateTime.UtcNow)
            {
                return BadRequest("El usuario logueado no tiene una suscripción activa.");
            }

            // Verificar si el usuario a vincular existe y no tiene una suscripción activa
            var usuarioVincular = await _context.Usuarios
                .Include(u => u.Suscripcion) // Incluir suscripción
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuarioVincular == null)
            {
                return BadRequest("El usuario a vincular no existe.");
            }

            if (usuarioVincular.Suscripcion != null && usuarioVincular.Suscripcion.FechaFin > DateTime.UtcNow)
            {
                return BadRequest("El usuario a vincular ya tiene una suscripción activa.");
            }

            // Realizar el vínculo
            var usuarioSuscripcion = new UsuarioSuscripcion
            {
                UsuarioCodigo = usuarioVincular.Codigo,
                SuscripcionCodigo = usuarioLogueado.Suscripcion.Codigo
            };

            _context.UsuariosSuscripciones.Add(usuarioSuscripcion);

            // Guardar los cambios
            await _context.SaveChangesAsync();

            return Ok("Usuario vinculado correctamente.");
        }


    }
}
