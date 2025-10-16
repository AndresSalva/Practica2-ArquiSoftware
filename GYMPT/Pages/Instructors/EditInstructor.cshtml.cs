using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Instructors
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IInstructorService _instructorService;
        private readonly UrlTokenSingleton _urlTokenSingleton;

        [BindProperty]
        public Instructor Instructor { get; set; }

        public EditModel(IInstructorService instructorService, UrlTokenSingleton urlTokenSingleton)
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
                return RedirectToPage("/Users/User");
            }
            Instructor = await _instructorService.GetInstructorById(id);

            if (Instructor == null)
            {
                TempData["ErrorMessage"] = "Instructor no encontrado.";
                return RedirectToPage("/Users/User");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _instructorService.UpdateInstructorData(Instructor);

            TempData["SuccessMessage"] = "Los datos del instructor han sido actualizados exitosamente.";
            return RedirectToPage("/Users/User");
        }
    }
}