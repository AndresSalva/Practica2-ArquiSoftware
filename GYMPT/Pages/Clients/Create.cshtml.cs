using GYMPT.Data.Contracts;
using GYMPT.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Clients
{
    public class CreateModel : PageModel
    {
        private readonly IClientRepository _clientRepo;

        [BindProperty]
        public Client Client { get; set; } = new Client();

        public CreateModel(IClientRepository clientRepo)
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

            return RedirectToPage("/Users");
        }
    }
}
