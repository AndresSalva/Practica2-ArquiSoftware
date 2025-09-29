using GYMPT.Data;
using GYMPT.Services;
using GYMPT.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseKey = builder.Configuration["Supabase:ApiKey"];

builder.Services.AddScoped(provider =>
    new Supabase.Client(
        supabaseUrl,
        supabaseKey,
        new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }));

builder.Services.AddScoped<MembershipRepository>();
builder.Services.AddScoped<DisciplineRepository>();

var app = builder.Build();

RemoteLoggerSingleton.Initialize(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();