using GYMPT.Models;
using GYMPT.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages
{
    public class ClientDetailsModel : PageModel
    {
        private readonly ClientRepository _clientRepo;

        public Client Client { get; set; }

        public ClientDetailsModel(ClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }


        public async Task<IActionResult> OnGetAsync(int id)
        {
   
            Client = await _clientRepo.GetByIdAsync(id);

            if (Client == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}