using ServiceCommon.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;

namespace GYMPT.Pages.Clients
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly ParameterProtector _urlTokenSingleton;

        [BindProperty]
        public Client Client { get; set; } = default!;

        public EditModel(IClientService clientService, ParameterProtector urlTokenSingleton)
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

            Client = await _clientService.GetByIdAsync(id);

            if (Client == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToPage("/Persons/Person");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _clientService.UpdateAsync(Client);

            TempData["SuccessMessage"] = "Los datos del cliente han sido actualizados exitosamente.";
            return RedirectToPage("/Persons/Person");
        }
    }
}