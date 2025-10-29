using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using ServiceUser.Domain.Entities;

namespace GYMPT.Pages.Clients
{
    [Authorize]
    public class ClientModel : PageModel
    {
        private readonly IClientService _userService;

        public IEnumerable<Client> ClientList { get; set; } = new List<Client>();
        public Dictionary<int, string> UserTokens { get; set; } = new Dictionary<int, string>();
        private readonly UrlTokenSingleton _urlTokenSingleton;

        public ClientModel(IClientService userService, UrlTokenSingleton urlTokenSingleton)
        {
            _userService = userService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task OnGetAsync()
        {
            ClientList = await _userService.GetAllClients();
            UserTokens = ClientList.ToDictionary(u => u.Id, u => _urlTokenSingleton.GenerateToken(u.Id.ToString()));
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var userToDelete = await _userService.GetClientById(id);
                if (userToDelete == null)
                {
                    TempData["ErrorMessage"] = "El usuario que intentas eliminar ya no existe.";
                    return RedirectToPage();
                }

                var success = _userService.DeleteClient(id);

                if (success.IsCompleted)
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