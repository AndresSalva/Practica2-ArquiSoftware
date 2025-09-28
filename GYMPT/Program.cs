using GYMPT.Data;

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


builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();



var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();