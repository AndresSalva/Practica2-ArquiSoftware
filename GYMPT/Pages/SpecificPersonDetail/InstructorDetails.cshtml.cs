using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceUser.Domain.Entities;
using ServiceUser.Application.Interfaces;
using ServiceCommon.Infrastructure.Services;

namespace GYMPT.Pages.SpecificUserDetail
{
    [Authorize(Roles = "Admin")]
    public class InstructorDetailsModel : PageModel
    {
        private readonly IUserService _instructorService;
        private readonly ParameterProtector _urlTokenSingleton;

        public User Instructor { get; set; }

        public string Token { get; set; }

        public InstructorDetailsModel(IUserService instructorService, ParameterProtector urlTokenSingleton)
        {
            _instructorService = instructorService;
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

            var instructorResult= await _instructorService.GetUserById(id);

            if (instructorResult.IsFailure)
            {
                TempData["ErrorMessage"] = "Instructor no encontrado.";
                return RedirectToPage("/Persons/Person");
            }
            Instructor = instructorResult.Value;
            return Page();
        }
    }
}
