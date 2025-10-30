using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using ServiceCommon.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceUser.Domain.Entities;
using ServiceUser.Application.Interfaces;

namespace GYMPT.Pages.SpecificUserDetail
{
    [Authorize(Roles = "Admin")]
    public class InstructorDetailsModel : PageModel
    {
        private readonly IUserService _instructorService;
        private readonly UrlTokenSingleton _urlTokenSingleton;

        public User Instructor { get; set; }

        // ✅ Agregar propiedad Token
        public string Token { get; set; }

        public InstructorDetailsModel(IUserService instructorService, UrlTokenSingleton urlTokenSingleton)
        {
            _instructorService = instructorService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            Token = token; // Guardamos el token para la vista

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
    }
}
