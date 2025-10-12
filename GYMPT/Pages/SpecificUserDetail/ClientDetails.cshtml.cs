using GYMPT.Factories; // <-- Se a�ade el using de F�bricas
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.SpecificUserDetail
{
    public class ClientDetailsModel : PageModel
    {
        public Client Client { get; set; }

        public ClientDetailsModel()
        {
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var factory = new ClientRepositoryCreator();
            var clientRepo = factory.CreateRepository();

            Client = await clientRepo.GetByIdAsync(id);

            if (Client == null)
            {

                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToPage("/Users/User");
            }

            return Page();
        }
    }
}