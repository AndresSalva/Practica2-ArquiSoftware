using GYMPT.Data.Contracts;
using GYMPT.Factories; 
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Clients
{
    public class EditClientModel : PageModel
    {
        public EditClientModel() { }

        [BindProperty]
        public Client Client { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        private IRepository<Client> CreateClientRepository()
        {
            var factory = new ClientRepositoryCreator();
            return factory.CreateRepository();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var clientRepo = CreateClientRepository(); 

            Client = await clientRepo.GetByIdAsync(Id);

            if (Client == null)
            {
                return RedirectToPage("/Users/User");
            }

            Id = Client.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Client.Id = Id;

            RepositoryCreator<Client> factory = new ClientRepositoryCreator();
            var clientRepo = factory.CreateRepository();

            await clientRepo.UpdateAsync(Client);

            TempData["Message"] = $"Cliente '{Client.Name}' actualizado correctamente.";
            return RedirectToPage("/Users/User");
        }
    }
}