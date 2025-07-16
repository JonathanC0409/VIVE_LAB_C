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
            CRUD<Song>.EndPoint = "https://localhost:7008/api/Songs";
            CRUD<User>.EndPoint = "https://localhost:7008/api/Users";
            CRUD<Plan>.EndPoint = "https://localhost:7008/api/Plans";
            CRUD<Subscription>.EndPoint = "https://localhost:7008/api/Subscriptions";
            CRUD<Playlist>.EndPoint = "https://localhost:7008/api/Playlists";
            CRUD<PlaylistSong>.EndPoint = "https://localhost:7008/api/PlaylistSongs";
            CRUD<UserSubscription>.EndPoint = "https://localhost:7008/api/UsersSubscriptions";
            CRUD<Album>.EndPoint = "https://localhost:7008/api/Albums";
            var builder = WebApplication.CreateBuilder(args);

            //Registrar Servicios
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<ISuscripcionService, SuscripcionService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IPerfilService, PerfilService>();

            // Add services to the container.
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();  // ?? Habilita soporte para sesión
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

            app.UseSession(); // ?? Activa la sesión

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
