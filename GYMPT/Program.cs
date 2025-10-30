using ServiceCommon.Extensions;
using ServicePerson.Extensions;
using ServiceClient.Extensions;
using ServiceUser.Extensions;
using ServiceDiscipline.Extensions;
using ServiceMembership.Extensions;

var builder = WebApplication.CreateBuilder(args);

// --- COMMON ---
builder.Services.AddCommonModule();
builder.Services.AddDataProtection();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

// --- DOMAINS ---
builder.Services.AddPersonModule();
builder.Services.AddClientModule();
builder.Services.AddUserModule();
builder.Services.AddDisciplineModule();
builder.Services.AddMembershipModule();

// --- SECURITY ---
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

// --- PIPELINE ---
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
