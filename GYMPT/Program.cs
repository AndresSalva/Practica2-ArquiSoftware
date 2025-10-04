using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;
using GYMPT.Services;

var builder = WebApplication.CreateBuilder(args);

// --- SECCI�N DE CONFIGURACI�N DE SERVICIOS ---

builder.Services.AddRazorPages();

// Registramos los repositorios usando interfaces (arquitectura limpia)
builder.Services.AddScoped<IRepository<Membership>, MembershipRepository>();
builder.Services.AddScoped<IRepository<DetailsUser>, DetailUserRepository>();

builder.Services.AddScoped<IRepository<UserData>, UserRepository>();
builder.Services.AddScoped<IRepository<Discipline>, DisciplineRepository>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();

// --- FIN DE LA SECCI�N DE CONFIGURACI�N ---

var app = builder.Build();

// Configuraci�n del RemoteLoggerSingleton
RemoteLoggerSingleton.Configure(app.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
