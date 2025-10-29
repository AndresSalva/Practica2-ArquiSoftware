using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using ServiceCommon.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Clients
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly ParameterProtector _urlTokenSingleton;

        [BindProperty]
        public Client Client { get; set; }

        public EditModel(IClientService clientService, ParameterProtector urlTokenSingleton)
        {
            _clientService = clientService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        // Este m�todo se ejecuta al cargar la p�gina para rellenar el formulario
        public async Task<IActionResult> OnGetAsync(string token)
        {
            // Decode token to original id
            var idStr = _urlTokenSingleton.Unprotect(token);
            if (!int.TryParse(idStr, out var id))
            {
                TempData["ErrorMessage"] = "Token de URL inválido.";
                return RedirectToPage("/Users/User");
            }
            Client = await _clientService.GetClientById(id);

            if (Client == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToPage("/Users/User");
            }
            return Page();
        }

        // Este m�todo se ejecuta al guardar los cambios
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Asumo que tu IClientService tiene un m�todo UpdateClientData
            await _clientService.UpdateClientData(Client);

            TempData["SuccessMessage"] = "Los datos del cliente han sido actualizados exitosamente.";
            return RedirectToPage("/Users/User");
        }
    }
}