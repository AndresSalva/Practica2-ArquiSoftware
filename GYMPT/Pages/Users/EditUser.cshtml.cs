using GYMPT.Application.Interfaces;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;
using System.Threading.Tasks;

namespace GYMPT.Pages.Instructors
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly UrlTokenSingleton _urlTokenSingleton;

        [BindProperty]
        public User Instructor { get; set; }

        public EditModel(IUserService userService, UrlTokenSingleton urlTokenSingleton)
        {
            _userService = userService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        // Este método se ejecuta al cargar la página para rellenar el formulario
        public async Task<IActionResult> OnGetAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Persons/Person");

            // Decode token to original id
            var idStr = _urlTokenSingleton.GetTokenData(token);
            if (!int.TryParse(idStr, out var id))
            {
                TempData["ErrorMessage"] = "Token inválido.";
                return RedirectToPage("/Persons/Person");
            }

            Instructor = await _userService.GetUserById(id);

            if (Instructor == null)
            {
                TempData["ErrorMessage"] = "Instructor no encontrado.";
                return RedirectToPage("/Persons/Person");
            }

            return Page();
        }

        // Este método se ejecuta al guardar los cambios
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userService.UpdateUser(Instructor);
            TempData["SuccessMessage"] = "Datos actualizados";
            return RedirectToPage("/Persons/Person");
        }
    }
}
