using GYMPT.Application.Interfaces;
using GYMPT.Application.Services;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Factories;
using GYMPT.Infrastructure.Security;
using GYMPT.Domain.Entities;
using ServiceMembership.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Ensures correct configuration of the url token singleton.
builder.Services.AddDataProtection();
builder.Services.AddSingleton<ParameterProtector >();


// --- 2. SERVICIOS QUE PERMANECEN EN GYMPT ---
// Estos son los servicios que AÚN no se han movido a sus propios módulos.

// Factoría de repositorios para las entidades restantes.
builder.Services.AddScoped<RepositoryFactory>();

builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<RepositoryFactory>();
    return (IUserRepository)factory.CreateRepository<User>();
});

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

builder.Services.AddScoped<IClientRepository>(sp =>
{
    var factory = sp.GetRequiredService<RepositoryFactory>();
    return (IClientRepository)factory.CreateRepository<Client>();
});


builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IDisciplineService, DisciplineService>();
builder.Services.AddScoped<ISelectDataService, SelectDataService>();
builder.Services.AddMembershipModule(_ => ConnectionStringSingleton.Instance.PostgresConnection);


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
// Discipline Service
builder.Services.AddDisciplineModule(_ => ConnectionStringSingleton.Instance.PostgresConnection);

//Conexion de user, servicios necesarios de user
builder.Services.AddUserModule(_ => ConnectionStringSingleton.Instance.PostgresConnection);
builder.Services.AddScoped<ISelectDataFacade, SelectDataFacade>();
builder.Services.AddScoped<ISelectDataService, SelectDataService>();
builder.Services.AddScoped<PersonFacade>();
builder.Services.AddScoped<UserCreationFacade>();
builder.Services.AddHttpContextAccessor(); // necesario para leer el contexto del usuario
builder.Services.AddScoped<IUserContextService, UserContextService>();
//

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

builder.Services.AddSingleton<IRemoteLogger, RemoteLogger>();

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
