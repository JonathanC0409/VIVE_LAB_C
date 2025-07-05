using Azure.Storage.Blobs;
using Vivelab.API.Consume;
using Vivelab.Modelos;
using Vivelab.Servicios;
using Vivelab.Servicios.Interfaces;

namespace Vivelab.Presentacion_MVC_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CRUD<Cancion>.EndPoint = "https://localhost:7008/api/Canciones"; 
            CRUD<Usuario>.EndPoint = "https://localhost:7008/api/Usuarios"; 
            CRUD<Plan>.EndPoint = "https://localhost:7008/api/Planes"; 
            CRUD<Suscripcion>.EndPoint = "https://localhost:7008/api/Suscripciones"; 
            CRUD<Playlist>.EndPoint = "https://localhost:7008/api/Playlists"; 
            var builder = WebApplication.CreateBuilder(args);

            //Registrar Servicios
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IMetodoPagoService, MetodoPagoService>();
            builder.Services.AddScoped<ISuscripcionService, SuscripcionService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IPerfilService, PerfilService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication("Cookies") //cokies
                .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/Login/Index"; // Ruta de inicio de sesión
 

                });
            builder.Services.AddHttpContextAccessor(); // Para acceder al contexto HTTP en los servicios //cokies

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication(); // Habilitar autenticación antes de usar routing//cookies

            app.UseRouting();

            app.UseAuthorization();//cokies

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
