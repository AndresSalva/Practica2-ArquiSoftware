using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceUser.Domain.Entities;

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

            await _userService.DeleteUser(User.Id);

            TempData["SuccessMessage"] = $"Usuario '{User.Name}' eliminado correctamente.";
            return RedirectToPage("/Persons/Person");
        }
    }
}