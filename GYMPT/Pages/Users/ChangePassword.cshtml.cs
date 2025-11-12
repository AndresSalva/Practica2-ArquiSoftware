using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceCommon.Infrastructure.Services;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Rules;

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserService _instructorService;
        private readonly ParameterProtector _urlTokenService;
        [BindProperty]
        public int UserId { get; set; }
        [BindProperty] public string NewPassword { get; set; } = string.Empty;
        [BindProperty] public string RepeatPassword { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public ChangePasswordModel(IUserService instructorService, ParameterProtector urlToken)
        {
            _instructorService = instructorService;
            _urlTokenService = urlToken;
        }

        public async void OnGetAsync(string token)
        {
            var id = _urlTokenService.Unprotect(token);
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

            var passwordResult = PasswordRules.Validar(NewPassword);
            if (passwordResult.IsFailure)
            {
                Message = passwordResult.Error;
                return Page();
            }

            var success = await _instructorService.UpdatePassword(UserId, NewPassword);
            if (success.IsFailure)
            {
                Message =  "Error al actualizar la contraseña. Intenta nuevamente.";
            }
            Message = "Contraseña actualizada correctamente.";

            return Page();
        }
    }
}
