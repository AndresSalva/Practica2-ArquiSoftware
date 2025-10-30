// Ruta: GYMPT/Pages/Clients/CreateModel.cshtml.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using System; // Necesario para Exception
using System.Threading.Tasks;

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
            // La validaci�n del lado del cliente (y atributos como [Required]) se comprueba aqu�.
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // --- �AQU� EST� LA L�GICA DE MANEJO DE ERRORES! ---
            try
            {
                // 1. Intentamos ejecutar la l�gica de negocio.
                // Esta es la l�nea que lanzar� la ArgumentException si la validaci�n del dominio falla.
                await _clientService.CreateAsync(Client);

                // 2. Si no hay excepci�n, la operaci�n fue exitosa. Redirigimos al usuario.
                return RedirectToPage("./Index");
            }
            catch (ArgumentException ex)
            {
                // 3. Si atrapamos una ArgumentException, sabemos que fue un error de validaci�n
                //    lanzado a prop�sito desde nuestro ClientService.

                // A�adimos el mensaje de error de la excepci�n al estado del modelo.
                // Esto permite que el error se muestre en la vista.
                ModelState.AddModelError(string.Empty, ex.Message);

                // 4. Devolvemos la misma p�gina. El usuario ver� el formulario con los datos que
                //    ingres� y el mensaje de error en la parte superior.
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