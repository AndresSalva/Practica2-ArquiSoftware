using ServiceUser.Infrastructure.DependencyInjection;
using ServiceClient.Infrastructure.DependencyInjection;
using ServiceDiscipline.Infrastructure.DependencyInjection;
using ServiceMembership.Infrastructure.DependencyInjection;
using ServiceCommon.Domain.Ports;
using ServiceCommon.Infrastructure.Services;
using GYMPT.Application.Services;
using GYMPT.Infrastructure.Security;
using GYMPT.Application.Facades;
using ServiceCommon.Domain.Entities;
using GYMPT.Infrastructure.Facade;
using GYMPT.Application.Interfaces;
using GYMPT.Infrastructure.Providers;
using QuestPDF.Infrastructure;
using ReportService.Application.Interfaces;
using ReportService.Application.Services;
using ReportService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configuración base
builder.Services.AddDataProtection();
builder.Services.AddSingleton<ParameterProtector>();
builder.Services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
builder.Services.AddSingleton<ConnectionStringProvider>();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<EmailService>();

// Helper para obtener la conexión desde DI
string GetConnectionString(IServiceProvider sp) =>
    sp.GetRequiredService<ConnectionStringProvider>().GetPostgresConnection();

// Módulos independientes
builder.Services.AddUserModule(GetConnectionString);
builder.Services.AddClientModule(GetConnectionString);
builder.Services.AddDisciplineModule(GetConnectionString);
builder.Services.AddMembershipModule(GetConnectionString);

// === NUEVA CONFIGURACIÓN PARA REPORTES ===
builder.Services.AddScoped<ILogoProvider, LogoProvider>();
builder.Services.AddScoped<IPdfReportBuilder, InstructorPerformancePdfBuilder>();
builder.Services.AddScoped<IReportService, ReportService.Application.Services.ReportService>();

// Configurar QuestPDF (solo una vez al inicio)
QuestPDF.Settings.License = LicenseType.Community;

// Si tienes un módulo de reportes, puedes agregarlo así:
// builder.Services.AddReportsModule(GetConnectionString);

// Facades
builder.Services.AddScoped<ISelectDataService, SelectDataService>();
builder.Services.AddScoped<GYMPT.Application.Interfaces.ISelectDataService, GYMPT.Application.Services.SelectDataService>();
builder.Services.AddScoped<PersonFacade>();
builder.Services.AddScoped<UserCreationFacade>();

// Servicios transversales (seguridad, login, hashing, etc.)
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<CookieAuthService>();
builder.Services.AddScoped<ISelectDataFacade, SelectDataFacade>();

// Autenticación y autorización
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

// UI
builder.Services.AddRazorPages();

// === NUEVO: Controlador para los endpoints de reportes ===
builder.Services.AddControllers(); // Necesario para los endpoints API de reportes

// Loggs Terminal
builder.Services.AddSingleton<IRemoteLogger>(sp =>
{
    var connProvider = sp.GetRequiredService<ConnectionStringProvider>();
    var connString = connProvider.GetPostgresConnection();
    return new RemoteLogger(connString);
});

// Construcción del pipeline HTTP
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// === NUEVO: Mapear controladores para los endpoints de reportes ===
app.MapControllers(); // Esto permite que funcionen los endpoints API
app.MapRazorPages();

app.Run();