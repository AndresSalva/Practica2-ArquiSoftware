using GYMPT.Application.Interfaces;
using GYMPT.Application.Services;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Factories;
using GYMPT.Infrastructure.Services;
using GYMPT.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Ensures correct configuration of the url token singleton.
builder.Services.AddDataProtection();
builder.Services.AddSingleton<UrlTokenSingleton>();

builder.Services.AddScoped<IClientRepository>(serviceProvider => {
    var factory = new ClientRepositoryCreator();

    return (IClientRepository)factory.CreateRepository();
});

builder.Services.AddScoped<IUserRepository>(serviceProvider => {
    var factory = new UserRepositoryCreator();
    return (IUserRepository)factory.CreateRepository();
});

builder.Services.AddScoped<IInstructorRepository>(serviceProvider => {
    var factory = new InstructorRepositoryCreator();
    return (IInstructorRepository)factory.CreateRepository();
});

builder.Services.AddScoped<IDisciplineRepository>(serviceProvider => {
    var factory = new DisciplineRepositoryCreator();
    return (IDisciplineRepository)factory.CreateRepository();
});

builder.Services.AddScoped<IMembershipRepository>(serviceProvider => {
    var factory = new MembershipRepositoryCreator();
    return (IMembershipRepository)factory.CreateRepository();
});

builder.Services.AddScoped<IDetailUserRepository>(serviceProvider => {
    var factory = new DetailUserRepositoryCreator();
    return (IDetailUserRepository)factory.CreateRepository();
});



builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IDisciplineService, DisciplineService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IDetailUserService, DetailUserService>();



builder.Services.AddRazorPages();

var app = builder.Build();


RemoteLoggerSingleton.Configure();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();