using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;

namespace GYMPT.Pages.Clients
{
    public class CreateModel : PageModel
    {
        private readonly IClientService _clientService;

        public CreateModel(IClientService clientService)
        {
            _clientService = clientService;
        }

        [BindProperty]
        public Client Client { get; set; } = new Client(); // Es buena pr�ctica inicializarlo

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _clientService.CreateAsync(Client);
                return RedirectToPage("./Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            // Opcional: Podr�as atrapar otras excepciones m�s gen�ricas si la base de datos falla.
            // catch (Exception ex)
            // {
            //     ModelState.AddModelError(string.Empty, "Ocurri� un error inesperado en el servidor. Por favor, intente de nuevo.");
            //     return Page();
            // }
        }
    }
}