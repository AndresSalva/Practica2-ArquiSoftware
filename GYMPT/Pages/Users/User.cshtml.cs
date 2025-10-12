using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    public class UserModel : PageModel
    {
        private readonly IUserService _userService;

        public IEnumerable<User> UserList { get; set; } = new List<User>();

        public UserModel(IUserService userService)
        {
            _userService = userService;
        }

        // Este m�todo se ejecuta cuando la p�gina se carga para MOSTRAR los usuarios
        public async Task OnGetAsync()
        {
            UserList = await _userService.GetAllUsers();
        }

        // Este m�todo se ejecuta cuando se confirma la eliminaci�n para BORRAR al usuario
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
                    // Prepara el pop-up de �xito
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

            // Redirige de vuelta a la misma p�gina para refrescar la lista y mostrar el pop-up
            return RedirectToPage();
        }
    }
}