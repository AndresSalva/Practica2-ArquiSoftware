using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GYMPT.Application.Interfaces;
using GYMPT.Infrastructure.Services;

namespace GYMPT.Pages.Instructors
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly IInstructorService _instructorService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly UrlTokenSingleton _urlTokenService;
        [BindProperty]
        public int UserId { get; set; }

        [BindProperty]
        public string NewPassword { get; set; } = string.Empty;
        [BindProperty]
        public string RepeatPassword { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public ChangePasswordModel(IInstructorService instructorService, IPasswordHasher passwordHasher, UrlTokenSingleton urlToken)
        {
            _instructorService = instructorService;
            _passwordHasher = passwordHasher;
            _urlTokenService = urlToken;
        }

        public async void OnGetAsync(string token)
        {
            var id = _urlTokenService.GetTokenData(token);
            UserId = int.Parse(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(RepeatPassword))
            {
                Message = "Por favor, completa ambos campos de contraseña.";
                return Page();
            }
            if (NewPassword != RepeatPassword)
            {
                Message = "Las contraseñas no coinciden. Intenta nuevamente.";
                return Page();
            }
            var hashed = _passwordHasher.Hash(NewPassword);
            var success = await _instructorService.UpdatePasswordAsync(UserId, hashed);
            if (success)
            {
                Message = "Contraseña actualizada correctamente.";
            }
            else
            {
                Message = "Error al actualizar la contraseña. Intenta nuevamente.";
            }
            return Page();
        }

    }
}
