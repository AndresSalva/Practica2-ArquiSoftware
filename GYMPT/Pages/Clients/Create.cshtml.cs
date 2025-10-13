using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Clients
{
    // El nombre de la clase coincide con el nombre del archivo (Create.cshtml -> CreateModel)
    public class CreateModel : PageModel
    {
        private readonly IClientService _clientService;

        [BindProperty]
        public Client Client { get; set; } = new();

        public CreateModel(IClientService clientService)
        {
            _clientService = clientService;
        }

        public void OnGet()
        {
            Client.Role = "Client";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Client.Role = "Client";
            await _clientService.CreateNewClient(Client);

            TempData["SuccessMessage"] = $"El cliente '{Client.Name} {Client.FirstLastname}' ha sido creado exitosamente.";
            return RedirectToPage("/Users/User");
        }
    }
}