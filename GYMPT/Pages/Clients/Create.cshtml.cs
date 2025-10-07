using GYMPT.Data.Contracts;
using GYMPT.Factories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Clients
{
    public class CreateModel : PageModel
    {

        [BindProperty]
        public Client Client { get; set; }

        public CreateModel()
        {

        }
        private IRepository<Client> CreateClientRepository()
        {
            var factory = new ClientRepositoryCreator();
            return factory.CreateRepository();
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var clientRepo = CreateClientRepository();

            await clientRepo.CreateAsync(Client);

            return RedirectToPage("/Users/User");
        }
    }
}
