using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel()
    {
    }

    [BindProperty]
    public string UsernameOrEmail { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(UsernameOrEmail) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Ingrese todos los campos.";
            return Page();
        }

        var user = _context.Users
            .FirstOrDefault(u => u.Username == UsernameOrEmail || u.Email == UsernameOrEmail);

        if (user == null)
        {
            ErrorMessage = "Usuario no encontrado.";
            return Page();
        }

        // Verificar contraseña (suponiendo hash con BCrypt)
        if (!BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
        {
            ErrorMessage = "Contraseña incorrecta.";
            return Page();
        }

        // Crear claims para la sesión
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties { IsPersistent = true };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );

        return RedirectToPage("/Index");
    }
}
