using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using ServiceCommon.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class UserModel : PageModel
    {
        private readonly IUserService _userService;

        public IEnumerable<User> UserList { get; set; } = new List<User>();
        public Dictionary<int, string> UserTokens { get; set; } = new Dictionary<int, string>();
        private readonly ParameterProtector _urlTokenSingleton;

        public UserModel(IUserService userService, ParameterProtector urlTokenSingleton)
        {
            _userService = userService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task OnGetAsync()
        {
            UserList = await _userService.GetAllUsers();
            UserTokens = UserList.ToDictionary(u => u.Id, u => _urlTokenSingleton.Protect(u.Id.ToString()));
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