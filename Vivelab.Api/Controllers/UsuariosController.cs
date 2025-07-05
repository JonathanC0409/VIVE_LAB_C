using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var usuario = await _context.Usuarios.FindAsync(id);

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

        // GET: Verificar si un usuario está asociado a una suscripción
        [HttpGet("VerificarSuscripcionUsuario/{usuarioId}")]
        public async Task<ActionResult> VerificarSuscripcionUsuario(int usuarioId)
        {
            // Buscar al usuario
            var usuario = await _context.Usuarios
                .Include(u => u.Suscripcion)  // Incluir la suscripción del usuario (solo si es el propietario)
                .FirstOrDefaultAsync(u => u.Codigo == usuarioId);

            // Si no encontramos al usuario, respondemos con un error
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Si el usuario tiene una suscripción directamente asociada (es propietario)
            if (usuario.Suscripcion != null)
            {
                return Ok(new
                {
                    usuarioId = usuarioId,
                    suscripcionCodigo = usuario.Suscripcion.Codigo,
                    estadoSuscripcion = usuario.Suscripcion.Estado,
                    fechaInicio = usuario.Suscripcion.FechaInicio,
                    fechaFin = usuario.Suscripcion.FechaFin,
                    esPropietario = true // Indicar que el usuario es el propietario
                });
            }

            // Si el usuario no es el propietario, verificamos si es un usuario adicional
            var usuarioSuscripcion = await _context.UsuariosSuscripciones
                .Include(us => us.Suscripcion) // Incluir la suscripción asociada
                .FirstOrDefaultAsync(us => us.UsuarioCodigo == usuarioId);

            // Si no se encuentra ninguna relación, significa que el usuario no está asociado a ninguna suscripción
            if (usuarioSuscripcion == null)
            {
                return NotFound("El usuario no está asociado a ninguna suscripción.");
            }

            // Si es un usuario adicional, obtenemos la suscripción relacionada
            var suscripcion = usuarioSuscripcion.Suscripcion;
            return Ok(new
            {
                usuarioId = usuarioId,
                suscripcionCodigo = suscripcion.Codigo,
                estadoSuscripcion = suscripcion.Estado,
                fechaInicio = suscripcion.FechaInicio,
                fechaFin = suscripcion.FechaFin,
                esPropietario = false // Indicar que el usuario es un usuario adicional
            });
        }

        [HttpPost("ComprarSuscripcion/{usuarioId}")]
        public async Task<ActionResult> ComprarSuscripcion(int usuarioId)
        {
            // Obtener al usuario adicional y su suscripción actual
            var usuarioAdicional = await _context.Usuarios
                .Include(u => u.Suscripcion)
                .FirstOrDefaultAsync(u => u.Codigo == usuarioId);

            if (usuarioAdicional == null || usuarioAdicional.Suscripcion != null)
            {
                return NotFound("El usuario no tiene una suscripción compartida.");
            }

            // Eliminar la relación en la tabla UsuarioSuscripcion entre el usuario adicional y la suscripción compartida
            var usuarioSuscripcion = await _context.UsuariosSuscripciones
                .FirstOrDefaultAsync(us => us.UsuarioCodigo == usuarioId);

            if (usuarioSuscripcion != null)
            {
                _context.UsuariosSuscripciones.Remove(usuarioSuscripcion);
                await _context.SaveChangesAsync();
            }

            // Crear una nueva suscripción para el usuario adicional (independiente)
            var nuevaSuscripcion = new Suscripcion
            {
                FechaInicio = DateTime.UtcNow,  // Usamos UTC en lugar de Local
                FechaFin = DateTime.UtcNow.AddMonths(1),  // Usamos UTC también aquí
                Estado = "activo",
                UsuarioCodigo = usuarioAdicional.Codigo,
                PlanCodigo = 2,  // El nuevo plan seleccionado por el usuario

            };

            _context.Suscripciones.Add(nuevaSuscripcion);
            await _context.SaveChangesAsync();



            return Ok("El usuario ha comprado su propia suscripción.");
        }


    }
}
