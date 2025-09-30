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

        // Esta propiedad pública guardará el objeto Client para que la vista pueda usarlo.
        public Client Client { get; set; }

        // Inyectamos el repositorio específico para clientes.
        public ClientDetailsModel(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }

        // Este método se ejecuta cuando se carga la página (HTTP GET).
        // El parámetro 'id' se toma automáticamente de la URL.
        public async Task<IActionResult> OnGetAsync(long id)
        {
            // Llamamos al repositorio para obtener el cliente completo.
            Client = await _clientRepo.GetByIdAsync(id);

            // Si el repositorio devuelve null (cliente no encontrado),
            // devolvemos una página de error 404 Not Found.
            if (Client == null)
            {
                return NotFound();
            }

            // Si todo va bien, mostramos la página.
            return Page();
        }
    }
}