using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;
using GYMPT.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GYMPT.Data;

var builder = WebApplication.CreateBuilder(args);

// --- SECCI�N DE CONFIGURACI�N DE SERVICIOS ---

builder.Services.AddRazorPages();
builder.Services.AddDbContext<GYMPTContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GYMPTContext") ?? throw new InvalidOperationException("Connection string 'GYMPTContext' not found.")));

// 1. ELIMINAMOS TODA LA CONFIGURACI�N DE SUPABASE
// La conexi�n a la base de datos ahora la gestiona cada repositorio
// leyendo la cadena de conexi�n que pusimos en appsettings.json.

// 2. REGISTRAMOS LOS REPOSITORIOS (Esto no cambia)
// La aplicaci�n sigue funcionando con las interfaces, por lo que esta parte
// no necesita saber que hemos cambiado de Supabase a PostgreSQL. �Esa es la ventaja de una buena arquitectura!
builder.Services.AddScoped<IRepository<Membership>, MembershipRepository>();
builder.Services.AddScoped<IRepository<DetailsUser>, DetailUserRepository>();
builder.Services.AddScoped<IRepository<UserData>, UserRepository>();
builder.Services.AddScoped<IRepository<Discipline>, DisciplineRepository>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();

// --- FIN DE LA SECCI�N DE CONFIGURACI�N ---

var app = builder.Build();

// 3. CONFIGURAMOS NUESTRO LOGGER ADAPTADO
// En lugar de Initialize, llamamos a nuestro nuevo m�todo Configure y le pasamos
// toda la configuraci�n de la aplicaci�n para que pueda encontrar la cadena de conexi�n.
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