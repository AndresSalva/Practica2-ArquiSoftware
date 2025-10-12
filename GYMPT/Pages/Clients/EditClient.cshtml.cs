using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Clients
{
    public class EditClientModel : PageModel
    {
        private readonly IClientService _clientService;

        public EditClientModel(IClientService clientService)
        {
            _clientService = clientService;
        }

        [BindProperty]
        public Client Client { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Client = await _clientService.GetClientById(Id);

            if (Client == null)
            {
                TempData["ErrorMessage"] = "El cliente que intentas editar no fue encontrado.";
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

            Client.Id = Id;
            await _clientService.UpdateClientData(Client);
            TempData["SuccessMessage"] = $"Cliente '{Client.Name}' actualizado correctamente.";
            return RedirectToPage("/Users/User");
        }
    }
}