using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GYMPT.Application.Services;
using GYMPT.Infrastructure.Security;
using GYMPT.Domain.Entities;

namespace GYMPT.Pages.Login;

public class IndexModel : PageModel
{
    private readonly LoginService _loginService;
    private readonly CookieAuthService _authService;

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    public IndexModel(LoginService loginService, CookieAuthService authService)
    {
        _loginService = loginService;
        _authService = authService;
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

        await _authService.SignInAsync(user);

        return RedirectToPage("/Index");
    }
}
