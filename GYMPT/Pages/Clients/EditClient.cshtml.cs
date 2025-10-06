using GYMPT.Data.Repositories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Clients
{
    public class EditClientModel : PageModel
    {
        private readonly ClientRepository _clientRepo;

        public EditClientModel(ClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }

        [BindProperty]
        public Client Client { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Client = await _clientRepo.GetByIdAsync(Id);
            if (Client == null) return RedirectToPage("/Users/User");
            Id = Client.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Client == null || Id == 0) return RedirectToPage("/Users");
            Client.Id = Id;
            var updated = await _clientRepo.UpdateAsync(Client);

            TempData["Message"] = $"Cliente {Client.Name} actualizado correctamente.";
            return RedirectToPage("/Users/User");
        }
    }
}
