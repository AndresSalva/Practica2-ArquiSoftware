using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GYMPT.Application.Interfaces;
<<<<<<< HEAD:GYMPT/Pages/Instructors/ChangePassword.cshtml.cs
using ServiceCommon.Infrastructure.Services;
=======
using GYMPT.Infrastructure.Services;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Rules; // 👈 Importante
using ServiceUser.Application.Common; // Para Result<T>
>>>>>>> Service-Usuario:GYMPT/Pages/Users/ChangePassword.cshtml.cs

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserService _instructorService;
        private readonly IPasswordHasher _passwordHasher;
<<<<<<< HEAD:GYMPT/Pages/Instructors/ChangePassword.cshtml.cs
        private readonly ParameterProtector _urlTokenService;
        [BindProperty]
        public int UserId { get; set; }
=======
        private readonly UrlTokenSingleton _urlTokenService;
>>>>>>> Service-Usuario:GYMPT/Pages/Users/ChangePassword.cshtml.cs

        [BindProperty] public int UserId { get; set; }
        [BindProperty] public string NewPassword { get; set; } = string.Empty;
        [BindProperty] public string RepeatPassword { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

<<<<<<< HEAD:GYMPT/Pages/Instructors/ChangePassword.cshtml.cs
        public ChangePasswordModel(IInstructorService instructorService, IPasswordHasher passwordHasher, ParameterProtector urlToken)
=======
        public ChangePasswordModel(
            IUserService instructorService,
            IPasswordHasher passwordHasher,
            UrlTokenSingleton urlToken)
>>>>>>> Service-Usuario:GYMPT/Pages/Users/ChangePassword.cshtml.cs
        {
            _instructorService = instructorService;
            _passwordHasher = passwordHasher;
            _urlTokenService = urlToken;
        }

        public async void OnGetAsync(string token)
        {
            var id = _urlTokenService.Unprotect(token);
            UserId = int.Parse(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 🧩 Validar campos vacíos
            if (string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(RepeatPassword))
            {
                Message = "Por favor, completa ambos campos de contraseña.";
                return Page();
            }

            // 🧩 Validar coincidencia
            if (NewPassword != RepeatPassword)
            {
                Message = "Las contraseñas no coinciden. Intenta nuevamente.";
                return Page();
            }

            // 🧩 Validar reglas de complejidad con PasswordRules
            var passwordResult = PasswordRules.Validar(NewPassword);
            if (passwordResult.IsFailure)
            {
                Message = passwordResult.Error; // muestra la causa exacta
                return Page();
            }

            // 🧩 Hashear y actualizar
            var hashed = _passwordHasher.Hash(passwordResult.Value);
            var success = await _instructorService.UpdatePasswordAsync(UserId, hashed);

            Message = success
                ? "Contraseña actualizada correctamente."
                : "Error al actualizar la contraseña. Intenta nuevamente.";

            return Page();
        }
    }
}
