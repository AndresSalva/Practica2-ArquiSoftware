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

var builder = WebApplication.CreateBuilder(args);

// Configuración base
builder.Services.AddDataProtection();
builder.Services.AddSingleton<ParameterProtector>();
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
app.MapRazorPages();

app.Run();
