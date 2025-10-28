// --- Usings Limpios y Correctos ---
using GYMPT.Application.Interfaces;
using GYMPT.Application.Services;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Factories;
using GYMPT.Infrastructure.Services;
using GYMPT.Infrastructure.Security;
using GYMPT.Domain.Entities;
// ¡El using clave para la integración!
using ServiceClient.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// --- 1. INTEGRACIÓN DEL MÓDULO ServiceClient ---
// Esta única línea registra IClientRepository, IUserRepository, IClientService, IUserService
// y toda la configuración de conexión necesaria para el módulo de cliente.
builder.Services.AddClientModule(provider =>
    // <-- CAMBIO REALIZADO AQUÍ
    // Se cambió "PostgresConnection" por "Postgres" para que coincida con tu appsettings.json
    builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("La cadena de conexión 'Postgres' no se encontró."));


// --- 2. SERVICIOS QUE PERMANECEN EN GYMPT ---
// Estos son los servicios que AÚN no se han movido a sus propios módulos.

// Factoría de repositorios para las entidades restantes.
builder.Services.AddScoped<RepositoryFactory>();

// Repositorios restantes (Instructor, Disciplina, etc.)
builder.Services.AddScoped<IInstructorRepository>(sp =>
{
    var factory = sp.GetRequiredService<RepositoryFactory>();
    return (IInstructorRepository)factory.CreateRepository<Instructor>();
});
builder.Services.AddScoped<IDisciplineRepository>(sp =>
{
    var factory = sp.GetRequiredService<RepositoryFactory>();
    return (IDisciplineRepository)factory.CreateRepository<Discipline>();
});
builder.Services.AddScoped<IMembershipRepository>(sp =>
{
    var factory = sp.GetRequiredService<RepositoryFactory>();
    return (IMembershipRepository)factory.CreateRepository<Membership>();
});
builder.Services.AddScoped<IDetailUserRepository>(sp =>
{
    var factory = sp.GetRequiredService<RepositoryFactory>();
    return (IDetailUserRepository)factory.CreateRepository<DetailsUser>();
});

// Servicios de aplicación restantes.
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IDisciplineService, DisciplineService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IDetailUserService, DetailUserService>();
builder.Services.AddScoped<ISelectDataService, SelectDataService>();


// --- 3. SERVICIOS DE UI Y SEGURIDAD (Se mantienen sin cambios) ---
builder.Services.AddDataProtection();
builder.Services.AddSingleton<UrlTokenSingleton>();

builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<CookieAuthService>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<EmailService>();
builder.Services.AddRazorPages();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Index";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();


// --- 4. CONFIGURACIÓN DEL PIPELINE HTTP (Se mantiene sin cambios) ---
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();