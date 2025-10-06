using GYMPT.Data.Contracts;
using GYMPT.Domain;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Clients
{
    public class EditClientModel : PageModel
    {
        private readonly IClientRepository _clientRepo;

        public EditClientModel(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }

        [BindProperty]
        public Client Client { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == 0) return RedirectToPage("/Users");

            Client = await _clientRepo.GetByIdAsync(Id);
            if (Client == null) return RedirectToPage("/Users");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Client == null || Id == 0) return RedirectToPage("/Users");

            await _clientRepo.UpdateAsync(Client);

            TempData["Message"] = $"Cliente {Client.Name} actualizado correctamente.";
            return RedirectToPage("/Users");
        }
    }
}
