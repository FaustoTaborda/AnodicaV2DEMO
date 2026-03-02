using Anodica.AccesoDatos;
using Anodica.AccesoDatos.Repositorio;
using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// SERILOG - Para registrar errores en archivo de log interno
builder.Host.UseSerilog((context, configuration) =>
    configuration.WriteTo.Console()
                 .WriteTo.File("Logs/log-errores-.txt", rollingInterval: RollingInterval.Day) //RollingInterval para crear un nuevo archivo cada día y que no se junte todo en uno solo
);

// 1. Configuración de Controladores y Vistas
builder.Services.AddControllersWithViews();

// 2. Configuración de la Base de Datos SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Inyección de Dependencias (Capa de Acceso a Datos)
builder.Services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();

var app = builder.Build();

// --- Configuración del Pipeline HTTP ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Optimización moderna de archivos estáticos
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();