using Microsoft.AspNetCore.Mvc.RazorPages;
using GYMPT.Infrastructure.Security;

namespace GYMPT.Pages.Login;

public class LogoutModel : PageModel
{
    private readonly CookieAuthService _authService;

    public LogoutModel(CookieAuthService authService)
    {
        _authService = authService;
    }

    public async Task OnGet()
    {
        await _authService.SignOutAsync();
        Response.Redirect("/Login/Index");
    }
}
