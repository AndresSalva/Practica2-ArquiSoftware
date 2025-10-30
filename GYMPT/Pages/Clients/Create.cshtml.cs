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
        public Client Client { get; set; } = new Client(); // Es buena práctica inicializarlo

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // La validación del lado del cliente (y atributos como [Required]) se comprueba aquí.
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // --- ¡AQUÍ ESTÁ LA LÓGICA DE MANEJO DE ERRORES! ---
            try
            {
                // 1. Intentamos ejecutar la lógica de negocio.
                // Esta es la línea que lanzará la ArgumentException si la validación del dominio falla.
                await _clientService.CreateAsync(Client);

                // 2. Si no hay excepción, la operación fue exitosa. Redirigimos al usuario.
                return RedirectToPage("./Index");
            }
            catch (ArgumentException ex)
            {
                // 3. Si atrapamos una ArgumentException, sabemos que fue un error de validación
                //    lanzado a propósito desde nuestro ClientService.

                // Añadimos el mensaje de error de la excepción al estado del modelo.
                // Esto permite que el error se muestre en la vista.
                ModelState.AddModelError(string.Empty, ex.Message);

                // 4. Devolvemos la misma página. El usuario verá el formulario con los datos que
                //    ingresó y el mensaje de error en la parte superior.
                return Page();
            }
            // Opcional: Podrías atrapar otras excepciones más genéricas si la base de datos falla.
            // catch (Exception ex)
            // {
            //     ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado en el servidor. Por favor, intente de nuevo.");
            //     return Page();
            // }
        }
    }
}