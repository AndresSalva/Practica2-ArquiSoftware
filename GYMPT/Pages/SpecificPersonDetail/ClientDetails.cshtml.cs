using ServiceCommon.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;

namespace GYMPT.Pages.SpecificUserDetail
{
    [Authorize]
    public class ClientDetailsModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly ParameterProtector _urlTokenSingleton;
        public Client Client { get; set; }

        public ClientDetailsModel(IClientService clientService, ParameterProtector urlTokenSingleton)
        {
            _clientService = clientService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            var idStr = _urlTokenSingleton.Unprotect(token);
            if (!int.TryParse(idStr, out var id))
            {
                TempData["ErrorMessage"] = "Token de URL inválido.";
                return RedirectToPage("/Persons/Person");
            }

            // --- CAMBIO 2: Usar el nombre de método correcto del nuevo contrato ---
            Client = await _clientService.GetByIdAsync(id);

            if (Client == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToPage("/Persons/Person");
            }

            return Page();
        }
    }
}