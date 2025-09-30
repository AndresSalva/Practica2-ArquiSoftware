using GYMPT.Data.Contracts;
using GYMPT.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages
{
    public class ClientDetailsModel : PageModel
    {
        private readonly IClientRepository _clientRepo;

        public Client Client { get; set; }

        public ClientDetailsModel(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }


        public async Task<IActionResult> OnGetAsync(long id)
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