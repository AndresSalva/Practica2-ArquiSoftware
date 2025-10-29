using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceUser.Application.Services;
using ServiceUser.Domain.Entities;
using ServiceUser.Application.Interfaces;

namespace GYMPT.Pages.Instructors
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IUserService _instructorService;
        private readonly UrlTokenSingleton _urlTokenSingleton;

        [BindProperty]
        public User Instructor { get; set; }

        public EditModel(IUserService instructorService, UrlTokenSingleton urlTokenSingleton)
        {
            _instructorService = instructorService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            var idStr = _urlTokenSingleton.GetTokenData(token);
            if (!int.TryParse(idStr, out var id))
            {
                TempData["ErrorMessage"] = "Token de URL inválido.";
                return RedirectToPage("/Persons/Person");
            }
            Instructor = await _instructorService.GetUserById(id);

            if (Instructor == null)
            {
                TempData["ErrorMessage"] = "Instructor no encontrado.";
                return RedirectToPage("/Persons/Person");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _instructorService.UpdateUser(Instructor);

            TempData["SuccessMessage"] = "Los datos del instructor han sido actualizados exitosamente.";
            return RedirectToPage("/Persons/Person");
        }
    }
}