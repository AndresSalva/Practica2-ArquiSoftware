using GYMPT.Data.Repositories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Clients
{
    public class CreateModel : PageModel
    {
        private readonly ClientRepository _clientRepo;

        [BindProperty]
        public Client Client { get; set; }

        public CreateModel(ClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Esto crea el User + Client en una sola transacción
            await _clientRepo.CreateAsync(Client);

            return RedirectToPage("/Users/User");
        }
    }
}
