using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;
using GYMPT.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GYMPT.Data;
using GYMPT.Domain;

var builder = WebApplication.CreateBuilder(args);

// --- Configuracion servicios ---

builder.Services.AddRazorPages();
builder.Services.AddDbContext<GYMPTContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GYMPTContext") ?? throw new InvalidOperationException("Connection string 'GYMPTContext' not found.")));

// Configuramos la inyección de dependencias para los repositorios
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<DisciplineRepository>();
builder.Services.AddScoped<MembershipRepository>();
builder.Services.AddScoped<DetailUserRepository>();

builder.Services.AddScoped<IRepository<UserData>, UserRepository>();
//builder.Services.AddScoped<IRepository<ClientData>, ClientRepository>();
//builder.Services.AddScoped<IRepository<InstructorData>, InstructorRepository>();
builder.Services.AddScoped<IRepository<Discipline>, DisciplineRepository>();
builder.Services.AddScoped<IRepository<Membership>, MembershipRepository>();
builder.Services.AddScoped<IRepository<DetailsUser>, DetailUserRepository>();

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