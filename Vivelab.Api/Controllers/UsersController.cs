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
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsuario()
        {
            var usuarios = await _context.Users
                .Include(u => u.UsersSubscriptions) // Incluir la relación con UsuariosSuscripciones
                .Include(u => u.Subscription) // Incluir la suscripción del usuario (si es el propietario)
                .Include(u => u.Subscription.Plan) // Incluir el plan de la suscripción
                .ToListAsync();

            return usuarios;

        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUsuario(int id)
        {
            var usuario = await _context.Users
              .Where(u => u.Code == id)
              .Include(u => u.Songs)
              .Include(u => u.Albums)
              .Include(u => u.Subscription)
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
        public async Task<IActionResult> PutUsuario(int id, User usuario)
        {
            if (id != usuario.Code)
            {
                return BadRequest();
            }

            // Obtener el usuario actual de la base de datos
            var usuarioExistente = await _context.Users.FindAsync(id);
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
        public async Task<ActionResult<User>> PostUsuario(User usuario)
        {
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            _context.Users.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Code }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Users.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Users.Any(e => e.Code == id);
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
            var usuarioLogueado = await _context.Users
                .Include(u => u.Subscription) // Incluir suscripción
                .Include(u => u.UsersSubscriptions)
                .FirstOrDefaultAsync(u => u.Email == emailLogeado);

            if (usuarioLogueado == null || usuarioLogueado.Subscription == null || usuarioLogueado.Subscription.EndDate <= DateTime.UtcNow)
            {
                return BadRequest("El usuario logueado no tiene una suscripción activa.");
            }

            // Verificar si el usuario a vincular existe y no tiene una suscripción activa
            var usuarioVincular = await _context.Users
                .Include(u => u.Subscription) // Incluir suscripción
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuarioVincular == null)
            {
                return BadRequest("El usuario a vincular no existe.");
            }

            var usarios = await _context.UsersSubscriptions
                .ToListAsync();

            foreach (var u in usarios)
            {
                if (u.UserCode == usuarioVincular.Code && u.SubscriptionCode == usuarioLogueado.Subscription.Code)
                {
                    return BadRequest("El usuario ya está vinculado a esta suscripción.");
                }
            }

            if (usuarioVincular.Subscription != null && usuarioVincular.Subscription.EndDate > DateTime.UtcNow)
            {
                return BadRequest("El usuario a vincular ya tiene una suscripción activa.");
            }

            // Realizar el vínculo
            var usuarioSuscripcion = new UserSubscription
            {
                UserCode = usuarioVincular.Code,
                SubscriptionCode = usuarioLogueado.Subscription.Code
            };

            _context.UsersSubscriptions.Add(usuarioSuscripcion);

            // Guardar los cambios
            await _context.SaveChangesAsync();

            return Ok("Usuario vinculado correctamente.");
        }



    }
}
