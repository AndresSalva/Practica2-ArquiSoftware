using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
// --- AGREGA ESTOS USINGS ---
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
// ---------------------------

namespace GYMPT.Pages.Clients
{
    public class CreateModel : PageModel
    {
        // Ahora IClientService se refiere correctamente a la interfaz del nuevo módulo
        private readonly IClientService _clientService;

        public CreateModel(IClientService clientService)
        {
            _clientService = clientService;
        }

        [BindProperty]
        public Client Client { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Client == null)
            {
                return Page();
            }

            // ANTES (incorrecto):
            // await _clientService.CreateClientAsync(Client);

            // DESPUÉS (correcto, asumiendo que el método se llama CreateAsync):
            await _clientService.CreateAsync(Client);

            return RedirectToPage("./Index");
        }
    }
}