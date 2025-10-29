using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Clients
{
    [Authorize]
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
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _clientService.CreateNewClient(Client);

            TempData["SuccessMessage"] = $"El cliente '{Client.Name} {Client.FirstLastname}' ha sido creado exitosamente.";
            return RedirectToPage("/Users/User");
        }
    }
}