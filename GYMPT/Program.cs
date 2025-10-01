using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;
using GYMPT.Services;

var builder = WebApplication.CreateBuilder(args);

// --- SECCIÓN DE CONFIGURACIÓN DE SERVICIOS ---

builder.Services.AddRazorPages();

// 1. ELIMINAMOS TODA LA CONFIGURACIÓN DE SUPABASE
// La conexión a la base de datos ahora la gestiona cada repositorio
// leyendo la cadena de conexión que pusimos en appsettings.json.

// 2. REGISTRAMOS LOS REPOSITORIOS (Esto no cambia)
// La aplicación sigue funcionando con las interfaces, por lo que esta parte
// no necesita saber que hemos cambiado de Supabase a PostgreSQL. ¡Esa es la ventaja de una buena arquitectura!
builder.Services.AddScoped<IRepository<Membership>, MembershipRepository>();
builder.Services.AddScoped<IRepository<DetailsUser>, DetailUserRepository>();
builder.Services.AddScoped<IRepository<UserData>, UserRepository>();
builder.Services.AddScoped<IRepository<Discipline>, DisciplineRepository>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();

// --- FIN DE LA SECCIÓN DE CONFIGURACIÓN ---

var app = builder.Build();

// 3. CONFIGURAMOS NUESTRO LOGGER ADAPTADO
// En lugar de Initialize, llamamos a nuestro nuevo método Configure y le pasamos
// toda la configuración de la aplicación para que pueda encontrar la cadena de conexión.
RemoteLoggerSingleton.Configure(app.Configuration);

// El resto del archivo no necesita cambios.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();