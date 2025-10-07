using GYMPT.Services;
using Microsoft.EntityFrameworkCore;
using GYMPT.Data;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();


builder.Services.AddDbContext<GYMPTContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GYMPTContext") ?? throw new InvalidOperationException("Connection string 'GYMPTContext' not found.")));


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