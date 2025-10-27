using GYMPT.Application.Interfaces;
using GYMPT.Application.Services;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Factories;
using GYMPT.Infrastructure.Services;
using GYMPT.Infrastructure.Security;
using GYMPT.Domain.Entities;
using ServiceDiscipline.Application.Interfaces;
using ServiceDiscipline.Application.Services;
using ServiceDiscipline.Infrastructure.Persistence;
using ServiceDiscipline.Domain.Ports;

var builder = WebApplication.CreateBuilder(args);

// Ensures correct configuration of the url token singleton.
builder.Services.AddDataProtection();
builder.Services.AddSingleton<UrlTokenSingleton>();

builder.Services.AddScoped<RepositoryFactory>();

builder.Services.AddScoped<IUserRepository>(sp =>
{
    var factory = sp.GetRequiredService<RepositoryFactory>();
    return (IUserRepository)factory.CreateRepository<User>();
});

builder.Services.AddScoped<IInstructorRepository>(sp =>
{
    var factory = sp.GetRequiredService<RepositoryFactory>();
    return (IInstructorRepository)factory.CreateRepository<Instructor>();
});

builder.Services.AddScoped<IClientRepository>(sp =>
{
    var factory = sp.GetRequiredService<RepositoryFactory>();
    return (IClientRepository)factory.CreateRepository<Client>();
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

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
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
// Discipline Service
builder.Services.AddScoped<IDisciplineRepository, DisciplineRepository>();
builder.Services.AddScoped<IDisciplineService, DisciplineService>();

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