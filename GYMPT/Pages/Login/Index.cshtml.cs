using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GYMPT.Application.Services;
using GYMPT.Infrastructure.Security;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;

namespace GYMPT.Pages.Login;

public class IndexModel : PageModel
{
    private readonly LoginService _loginService;
    private readonly CookieAuthService _authService;
    private readonly UrlTokenSingleton _urlTokenService;

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    public IndexModel(LoginService loginService, CookieAuthService authService, UrlTokenSingleton urlToken)
    {
        _loginService = loginService;
        _authService = authService;
        _urlTokenService = urlToken;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Debe ingresar su email y contraseña.";
            return Page();
        }

        var user = await _loginService.AuthenticateAsync(Email, Password);

        if (user == null)
        {
            ErrorMessage = "Credenciales inválidas.";
            return Page();
        }

        if (user.MustChangePassword)
        {
            await _authService.SignInAsync(user);
            string token = _urlTokenService.GenerateToken(user.Id.ToString());
            return RedirectToPage("/Instructors/ChangePassword", new { token });
        }

        await _authService.SignInAsync(user);

        return RedirectToPage("/Index");
    }
}
