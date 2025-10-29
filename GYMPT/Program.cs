using GYMPT.Application.Facades;
using GYMPT.Application.Interfaces;
using GYMPT.Application.Services;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Facade;
using GYMPT.Infrastructure.Factories;
using GYMPT.Infrastructure.Security;
using GYMPT.Infrastructure.Services;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Ports;
using ServiceUser.Infrastructure.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Ensures correct configuration of the url token singleton.
builder.Services.AddDataProtection();
builder.Services.AddSingleton<UrlTokenSingleton>();

builder.Services.AddScoped<RepositoryFactory>();

builder.Services.AddScoped<IPersonRepository>(sp =>
{
    var factory = sp.GetRequiredService<RepositoryFactory>();
    return (IPersonRepository)factory.CreateRepository<Person>();
});

builder.Services.AddScoped<IUserRepository>(sp =>
{
    var factory = sp.GetRequiredService<RepositoryFactory>();
    return (IUserRepository)factory.CreateRepository<User>();
});


builder.Services.AddScoped<IClientRepository>(sp =>
{
    var factory = sp.GetRequiredService<RepositoryFactory>();
    return (IClientRepository)factory.CreateRepository<Client>();
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

//Servicios de los demas servicios
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDisciplineService, DisciplineService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IDetailUserService, DetailUserService>();
builder.Services.AddScoped<ISelectDataService, SelectDataService>();

// Login Related Services
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<CookieAuthService>();
builder.Services.AddHttpContextAccessor();

// Email Credentials Related Services
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<EmailService>();
builder.Services.AddRazorPages();

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

var app = builder.Build();


RemoteLoggerSingleton.Configure();


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