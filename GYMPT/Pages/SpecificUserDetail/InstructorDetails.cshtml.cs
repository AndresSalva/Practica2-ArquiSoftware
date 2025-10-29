using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using ServiceCommon.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.SpecificUserDetail
{
    [Authorize(Roles = "Admin")]
    public class InstructorDetailsModel : PageModel
    {
        private readonly IInstructorService _instructorService;
        private readonly ParameterProtector _urlTokenSingleton;
        public Instructor Instructor { get; set; }

        public InstructorDetailsModel(IInstructorService instructorService, ParameterProtector urlTokenSingleton)
        {
            _instructorService = instructorService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            var idStr = _urlTokenSingleton.Unprotect(token);
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
    }
}