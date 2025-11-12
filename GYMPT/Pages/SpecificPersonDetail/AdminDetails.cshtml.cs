using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceCommon.Infrastructure.Services;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;

namespace GYMPT.Pages.SpecificUserDetail
{
    [Authorize(Roles = "Admin")]
    public class AdminDetailsModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ParameterProtector _urlTokenSingleton;

        public User Admin { get; set; }

        // Guardar token solo si necesitas pasarlo a otros enlaces en la vista
        public string Token { get; set; }

        public AdminDetailsModel(IUserService userService, ParameterProtector urlTokenSingleton)
        {
            _userService = userService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            Token = token;

            var idStr = _urlTokenSingleton.Unprotect(token);
            if (!int.TryParse(idStr, out var id))
            {
                TempData["ErrorMessage"] = "Token de URL inválido.";
                return RedirectToPage("/Persons/Person");
            }

            var adminResult = await _userService.GetUserById(id);

            if (adminResult.IsFailure)
            {
                TempData["ErrorMessage"] = "Administrador no encontrado.";
                return RedirectToPage("/Persons/Person");
            }
            Admin = adminResult.Value;
            return Page();
        }
    }
}
