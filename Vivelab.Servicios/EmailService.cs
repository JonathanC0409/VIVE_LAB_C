using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivelab.API.Consume;
using Vivelab.Modelos;
using Vivelab.Servicios.Interfaces;

namespace Vivelab.Servicios
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // Servidor SMTP de Gmail
        private readonly int _smptPort = 587; // Puerto SMTP para TLS
        private readonly string _fromEmail = "soyeljoni123@gmail.com"; // Correo electrónico del remitente (debe ser una cuenta de Gmail)
        private readonly string _fromPassword = "isst qlfk eetu anzt"; // Contraseña de la cuenta de correo electrónico

        public async Task enviarEmailBienvenida(string email)
        {
            try
            {
                var mensaje = new MimeMessage(); // Crear un nuevo mensaje MIME
                mensaje.From.Add(new MailboxAddress("Vivelab", _fromEmail)); // Establecer el remitente
                mensaje.To.Add(new MailboxAddress("",email)); // Establecer el destinatario
                mensaje.Subject = "Bienvenido a Vivelab - Ingreso Exitoso"; // Asunto del correo electrónico

                mensaje.Body = new TextPart("plain") // Cuerpo del correo electrónico
                {
                    Text = $"Hola,\n\n¡Bienvenido a Vivelab! Tu ingreso ha sido exitoso.\n\nSaludos,\nEl equipo de Vivelab."
                }; 

                using (var cliente = new SmtpClient())
                {
                    await cliente.ConnectAsync(_smtpServer, _smptPort, SecureSocketOptions.StartTls); // Conectar al servidor SMTP con TLS
                    await cliente.AuthenticateAsync(_fromEmail, _fromPassword); // Autenticar con el servidor SMTP usando las credenciales del remitente
                    await cliente.SendAsync(mensaje); // Enviar el mensaje
                    await cliente.DisconnectAsync(true); // Desconectar del servidor SMTP
                }

            }
            catch (Exception ex)
            {
                // Manejar excepciones de envío de correo electrónico
                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
            }
        }

        public async Task enviarEmailRecuperacionPassword(string email)
        {
            try
            {
                var tempPassword = Guid.NewGuid().ToString("N").Substring(0, 5); //Genera contraseña aletoria de 10 caracteres
                var mensaje = new MimeMessage(); // Crear un nuevo mensaje MIME
                mensaje.From.Add(new MailboxAddress("Vivelab", _fromEmail)); // Establecer el remitente
                mensaje.To.Add(new MailboxAddress("", email)); // Establecer el destinatario
                mensaje.Subject = "Recuperación de Contraseña - Vivelab"; // Asunto del correo electrónico

                // Cuerpo del mensaje
                mensaje.Body = new TextPart("plain")
                {
                    Text = $"Hola,\n\nHemos recibido una solicitud para recuperar tu contraseña. Aquí tienes una nueva contraseña temporal:\n\n{tempPassword}\n\nPor favor, cámbiala lo antes posible.\n\nSaludos,\nEl equipo de Vivelab."
                };
                using (var cliente = new SmtpClient())
                {
                    await cliente.ConnectAsync(_smtpServer, _smptPort, SecureSocketOptions.StartTls); // Conectar al servidor SMTP con TLS
                    await cliente.AuthenticateAsync(_fromEmail, _fromPassword); // Autenticar con el servidor SMTP usando las credenciales del remitente
                    await cliente.SendAsync(mensaje); // Enviar el mensaje
                    await cliente.DisconnectAsync(true); // Desconectar del servidor SMTP
                }

                var usuario = CRUD<Usuario>.GetAll().FirstOrDefault(u => u.Email == email); //Actualizar contraseña
                if (usuario != null)
                {
                    usuario.Password = tempPassword;
                    CRUD<Usuario>.Update(usuario.Codigo, usuario);
                    Console.WriteLine($"contraseña actualizada: {tempPassword}");

                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones de envío de correo electrónico
                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
            }
        }
    }
}
