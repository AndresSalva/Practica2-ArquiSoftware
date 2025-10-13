using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Clients
{
    // El nombre de la clase coincide con el nombre del archivo (Edit.cshtml -> EditModel)
    public class EditModel : PageModel
    {
        private readonly IClientService _clientService;

        [BindProperty]
        public Client Client { get; set; }

        public EditModel(IClientService clientService)
        {
            _clientService = clientService;
        }

        // Este método se ejecuta al cargar la página para rellenar el formulario
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Client = await _clientService.GetClientById(id);

            if (Client == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToPage("/Users/User");
            }
            return Page();
        }

        // Este método se ejecuta al guardar los cambios
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Asumo que tu IClientService tiene un método UpdateClientData
            await _clientService.UpdateClientData(Client);

            TempData["SuccessMessage"] = "Los datos del cliente han sido actualizados exitosamente.";
            return RedirectToPage("/Users/User");
        }
    }
}