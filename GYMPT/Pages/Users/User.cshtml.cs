// --- CAMBIO 1: Corregir las directivas 'using' ---
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

// Se necesita el 'using' del nuevo módulo para IUserService y User.
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;

// Se mantiene el 'using' para los servicios que siguen en GYMPT.
using GYMPT.Infrastructure.Services;

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class UserModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly UrlTokenSingleton _urlTokenSingleton;

        public IEnumerable<User> UserList { get; set; } = new List<User>();
        public Dictionary<int, string> UserTokens { get; set; } = new Dictionary<int, string>();

        public UserModel(IUserService userService, UrlTokenSingleton urlTokenSingleton)
        {
            _userService = userService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task OnGetAsync()
        {
            // --- CAMBIO 2: Estandarizar la llamada al método ---
            UserList = await _userService.GetAllAsync(); // El método correcto es GetAllAsync
            UserTokens = UserList.ToDictionary(u => u.Id, u => _urlTokenSingleton.GenerateToken(u.Id.ToString()));
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                // --- CAMBIO 2 (Continuación): Estandarizar las llamadas a los métodos ---
                var userToDelete = await _userService.GetByIdAsync(id); // El método correcto es GetByIdAsync
                if (userToDelete == null)
                {
                    TempData["ErrorMessage"] = "El usuario que intentas eliminar ya no existe.";
                    return RedirectToPage();
                }

                var success = await _userService.DeleteByIdAsync(id); // El método correcto es DeleteByIdAsync

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
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al intentar eliminar el usuario.";
            }

            return RedirectToPage();
        }
    }
}