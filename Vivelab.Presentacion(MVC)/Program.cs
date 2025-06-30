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
            CRUD<Cancion>.EndPoint = "https://localhost:7008/api/Canciones"; // Cambia la URL según tu configuración de API 
            CRUD<Usuario>.EndPoint = "https://localhost:7008/api/Usuarios"; // Cambia la URL según tu configuración de API
            CRUD<Plan>.EndPoint = "https://localhost:7008/api/Planes"; // Cambia la URL según tu configuración de API
            CRUD<Suscripcion>.EndPoint = "https://localhost:7008/api/Suscripciones"; // Cambia la URL según tu configuración de API
            var builder = WebApplication.CreateBuilder(args);

            //Registrar Servicios
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<ICancionService, CancionService>(serviceProvider =>
            {
                var blobService = serviceProvider.GetRequiredService<BlobServiceClient>();
                var containerName = builder.Configuration["AzureStorage:ContainerName"];
                return new CancionService(blobService, containerName);
            });
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
