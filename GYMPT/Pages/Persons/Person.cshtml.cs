using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using ServiceUser.Domain.Entities;

namespace GYMPT.Pages.Persons
{
    [Authorize]
    public class PersonModel : PageModel
    {
        private readonly IPersonService _userService;

        public IEnumerable<Person> UserList { get; set; } = new List<Person>();
        public Dictionary<int, string> UserTokens { get; set; } = new Dictionary<int, string>();
        private readonly UrlTokenSingleton _urlTokenSingleton;

        public PersonModel(IPersonService userService, UrlTokenSingleton urlTokenSingleton)
        {
            _userService = userService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task OnGetAsync()
        {
            UserList = await _userService.GetAllUsers();
            UserTokens = UserList.ToDictionary(u => u.Id, u => _urlTokenSingleton.GenerateToken(u.Id.ToString()));
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var userToDelete = await _userService.GetUserById(id);
                if (userToDelete == null)
                {
                    TempData["ErrorMessage"] = "El usuario que intentas eliminar ya no existe.";
                    return RedirectToPage();
                }

                var success = await _userService.DeleteUser(id);

                if (success)
                {
                    TempData["SuccessMessage"] = $"El usuario {userToDelete.Name} {userToDelete.FirstLastname} fue eliminado correctamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar al usuario. Es posible que ya haya sido eliminado.";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ocurri� un error inesperado al intentar eliminar el usuario.";
            }

            return RedirectToPage();
        }
    }
}