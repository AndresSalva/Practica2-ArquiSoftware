// --- CAMBIO 1: Corregir las directivas 'using' ---
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
<<<<<<< HEAD:GYMPT/Pages/Users/DeleteUser.cshtml.cs
using System.Threading.Tasks;

// Se necesita el 'using' del nuevo módulo para IUserService y User.
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
=======
using ServiceUser.Domain.Entities;
>>>>>>> Service-Usuario:GYMPT/Pages/Persons/DeletePerson.cshtml.cs

namespace GYMPT.Pages.Persons
{
    [Authorize]
    public class DeleteUserModel : PageModel
    {
        private readonly IPersonService _userService;

        [BindProperty]
<<<<<<< HEAD:GYMPT/Pages/Users/DeleteUser.cshtml.cs
        public User User { get; set; } = default!;

        public DeleteUserModel(IUserService userService)
=======
        public Person User { get; set; }
            
        public DeleteUserModel(IPersonService userService)
>>>>>>> Service-Usuario:GYMPT/Pages/Persons/DeletePerson.cshtml.cs
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0)
            {
                return RedirectToPage("/Persons/Person");
            }
<<<<<<< HEAD:GYMPT/Pages/Users/DeleteUser.cshtml.cs

            // --- CAMBIO 2: Estandarizar la llamada al método ---
            User = await _userService.GetByIdAsync(id); // El método correcto es GetByIdAsync
=======
            
            User = await _userService.GetUserById(id);
>>>>>>> Service-Usuario:GYMPT/Pages/Persons/DeletePerson.cshtml.cs

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

            // --- CAMBIO 2 (Continuación): Estandarizar la llamada al método ---
            await _userService.DeleteByIdAsync(User.Id); // El método correcto es DeleteByIdAsync

            TempData["SuccessMessage"] = $"Usuario '{User.Name}' eliminado correctamente.";
            return RedirectToPage("/Persons/Person");
        }
    }
}