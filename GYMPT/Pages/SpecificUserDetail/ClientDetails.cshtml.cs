using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.SpecificUserDetail
{
    public class ClientDetailsModel : PageModel
    {
        private readonly IClientService _clientService;
        public Client Client { get; set; }

        public ClientDetailsModel(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Client = await _clientService.GetClientById(id);

            if (Client == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToPage("/Users/User");
            }

            return Page();
        }
    }
}