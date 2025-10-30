using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ServiceClient.Application.Interfaces;
using ServiceUser.Application.Interfaces;
using ServiceCommon.Infrastructure.Services;
using ServiceUser.Domain.Entities;

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class UserModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IClientService _clientService;
        private readonly ParameterProtector _urlTokenSingleton;

        public IEnumerable<User> UserList { get; set; } = new List<User>();
        public Dictionary<int, string> UserTokens { get; set; } = new Dictionary<int, string>();

        public UserModel(IUserService userService, IClientService clientService, ParameterProtector urlTokenSingleton)
        {
            _userService = userService;
            _clientService = clientService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task OnGetAsync()
        {
            var systemUsers = await _userService.GetAllUsers();

            var clients = await _clientService.GetAllAsync();

            var combinedList = new List<User>();
            combinedList.AddRange(systemUsers);
            // TODO
            // combinedList.AddRange(clients);

            UserList = combinedList.OrderBy(u => u.FirstLastname).ThenBy(u => u.Name);

            UserTokens = UserList.ToDictionary(u => u.Id, u => _urlTokenSingleton.Protect(u.Id.ToString()));
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var success = await _userService.DeleteUser(id);

                if (!success)
                {
                    success = await _clientService.DeleteByIdAsync(id);
                }

                if (success)
                {
                    TempData["SuccessMessage"] = $"El registro con ID {id} fue eliminado correctamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar el registro. Es posible que ya haya sido eliminado.";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al intentar eliminar el registro.";
            }

            return RedirectToPage();
        }
    }
}