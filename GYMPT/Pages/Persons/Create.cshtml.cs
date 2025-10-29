using GYMPT.Infrastructure.Facade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ServiceUser.Domain.Entities;
using GYMPT.Application.DTO;

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UserCreationFacade _userCreationFacade;

        public CreateModel(UserCreationFacade userCreationFacade)
        {
            _userCreationFacade = userCreationFacade;
        }

        [BindProperty]
        public UserInputModel Input { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var success = await _userCreationFacade.CreateUserAsync(Input);

            if (success)
                TempData["SuccessMessage"] = $"El usuario '{Input.Name} {Input.FirstLastname}' ha sido creado exitosamente.";
            else
                TempData["ErrorMessage"] = "No se pudo crear el usuario.";

            return RedirectToPage("/Persons/Person");
        }
    }
}
