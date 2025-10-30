// --- CAMBIO 1: Actualizar las directivas 'using' ---
// Ya no usamos las interfaces y entidades de GYMPT, sino las del nuevo módulo ServiceClient.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceClient.Application.Interfaces; // <-- Usamos la nueva interfaz
using ServiceClient.Domain.Entities;      // <-- Usamos la nueva entidad
using GYMPT.Infrastructure.Services;      // Mantenemos esto para UrlTokenSingleton
using System.Threading.Tasks;             // Necesario para tareas asíncronas

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
            var idStr = _urlTokenSingleton.GetTokenData(token);
            if (!int.TryParse(idStr, out var id))
            {
                TempData["ErrorMessage"] = "Token de URL inválido.";
                return RedirectToPage("/Users/User");
            }

            // --- CAMBIO 2: Usar el nombre de método correcto del nuevo contrato ---
            // El método ahora se llama GetByIdAsync
            Client = await _clientService.GetByIdAsync(id);

            if (Client == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToPage("/Users/User");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // --- CAMBIO 3: Usar el nombre de método correcto del nuevo contrato ---
            // El método ahora se llama UpdateAsync
            await _clientService.UpdateAsync(Client);

            TempData["SuccessMessage"] = "Los datos del cliente han sido actualizados exitosamente.";
            return RedirectToPage("/Users/User");
        }
    }
}