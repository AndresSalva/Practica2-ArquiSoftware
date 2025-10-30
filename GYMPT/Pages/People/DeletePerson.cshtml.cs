// --- CAMBIO 1: Corregir las directivas 'using' ---
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

// Se necesita el 'using' del nuevo módulo para IUserService y User.
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class DeleteUserModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public User User { get; set; } = default!;

        public DeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0)
            {
                return RedirectToPage("/Users/User");
            }

            // --- CAMBIO 2: Estandarizar la llamada al método ---
            User = await _userService.GetByIdAsync(id); // El método correcto es GetByIdAsync

            if (User == null)
            {
                TempData["ErrorMessage"] = "El usuario que intentas eliminar no fue encontrado.";
                return RedirectToPage("/Users/User");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User?.Id == 0)
            {
                return RedirectToPage("/Users/User");
            }

            // --- CAMBIO 2 (Continuación): Estandarizar la llamada al método ---
            await _userService.DeleteByIdAsync(User.Id); // El método correcto es DeleteByIdAsync

            TempData["SuccessMessage"] = $"Usuario '{User.Name}' eliminado correctamente.";
            return RedirectToPage("/Users/User");
        }
    }
}