// --- CAMBIO 1: Corregir las directivas 'using' ---
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServicePerson.Application.Interfaces;
using ServicePerson.Domain.Entities;

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class DeleteUserModel : PageModel
    {
        private readonly IPersonService _userService;

        [BindProperty]
        public Person User { get; set; }
            
        public DeleteUserModel(IPersonService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0)
            {
                return RedirectToPage("/Persons/Person");
            }
            
            User = await _userService.GetUserById(id);

            if (User == null)
            {
                TempData["ErrorMessage"] = "El usuario que intentas eliminar no fue encontrado.";
                return RedirectToPage("/Persons/Person");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User?.Id == 0)
            {
                RedirectToPage("/Persons/Person");
            }

            // --- CAMBIO 2 (Continuaci�n): Estandarizar la llamada al m�todo ---
            await _userService.DeleteByIdAsync(User.Id); // El m�todo correcto es DeleteByIdAsync

            TempData["SuccessMessage"] = $"Usuario '{User.Name}' eliminado correctamente.";
            return RedirectToPage("/Persons/Person");
        }
    }
}