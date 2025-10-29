using GYMPT.Application.Interfaces;
<<<<<<< HEAD:GYMPT/Pages/Instructors/EditInstructor.cshtml.cs
using GYMPT.Domain.Entities;
using ServiceCommon.Infrastructure.Services;
=======
using GYMPT.Infrastructure.Services;
>>>>>>> Service-Usuario:GYMPT/Pages/Users/EditUser.cshtml.cs
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
<<<<<<< HEAD:GYMPT/Pages/Instructors/EditInstructor.cshtml.cs
        private readonly IInstructorService _instructorService;
        private readonly ParameterProtector _urlTokenSingleton;
=======
        private readonly IUserService _userService;
        private readonly UrlTokenSingleton _urlTokenSingleton;
>>>>>>> Service-Usuario:GYMPT/Pages/Users/EditUser.cshtml.cs

        [BindProperty]
        public User Instructor { get; set; }

<<<<<<< HEAD:GYMPT/Pages/Instructors/EditInstructor.cshtml.cs
        public EditModel(IInstructorService instructorService, ParameterProtector urlTokenSingleton)
=======
        public EditModel(IUserService userService, UrlTokenSingleton urlTokenSingleton)
>>>>>>> Service-Usuario:GYMPT/Pages/Users/EditUser.cshtml.cs
        {
            _userService = userService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        // Este método se ejecuta al cargar la página para rellenar el formulario
        public async Task<IActionResult> OnGetAsync(string token)
        {
<<<<<<< HEAD:GYMPT/Pages/Instructors/EditInstructor.cshtml.cs
            var idStr = _urlTokenSingleton.Unprotect(token);
=======
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Persons/Person");

            // Decode token to original id
            var idStr = _urlTokenSingleton.GetTokenData(token);
>>>>>>> Service-Usuario:GYMPT/Pages/Users/EditUser.cshtml.cs
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
