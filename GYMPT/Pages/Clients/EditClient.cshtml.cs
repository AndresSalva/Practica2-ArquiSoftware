using GYMPT.Data.Contracts;
using GYMPT.Factories;
using GYMPT.Models;
using GYMPT.Validations;
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
            // -------------------------
            // VALIDACIONES PERSONALIZADAS
            // -------------------------
            if (!ValidacionesUsuario.EsNombreCompletoValido(Client.Name))
                ModelState.AddModelError("Client.Name", "Nombre inválido. Debe tener mínimo 2 letras y solo letras y espacios.");

            if (!ValidacionesUsuario.EsNombreCompletoValido(Client.FirstLastname))
                ModelState.AddModelError("Client.FirstLastname", "Apellido Paterno inválido. Debe tener mínimo 2 letras y solo letras y espacios.");

            if (!ValidacionesUsuario.EsNombreCompletoValido(Client.SecondLastname))
                ModelState.AddModelError("Client.SecondLastname", "Apellido Materno inválido. Debe tener mínimo 2 letras y solo letras y espacios.");

            if (!ValidacionesUsuario.EsCiValido(Client.Ci))
                ModelState.AddModelError("Client.Ci", "CI inválido. Solo números y letras.");

            if (!ValidacionesUsuario.EsFechaNacimientoValida(Client.DateBirth))
                ModelState.AddModelError("Client.DateBirth", "Fecha de nacimiento inválida. No puede ser futura.");

            if (!ValidacionesUsuario.EsNivelFitnessValido(Client.FitnessLevel))
                ModelState.AddModelError("Client.FitnessLevel", "Nivel de fitness inválido. Debe ser: Principiante, Intermedio o Avanzado.");

            if (!ValidacionesUsuario.EsPesoValido(Client.InitialWeightKg))
                ModelState.AddModelError("Client.InitialWeightKg", "Peso inicial inválido. Debe ser mayor a 0.");

            if (!ValidacionesUsuario.EsPesoActualValido(Client.InitialWeightKg, Client.CurrentWeightKg))
                ModelState.AddModelError("Client.CurrentWeightKg", "Peso actual no puede ser menor al peso inicial.");

            if (!ValidacionesUsuario.EsTelefonoEmergenciaValido(Client.EmergencyContactPhone))
                ModelState.AddModelError("Client.EmergencyContactPhone", "Teléfono de emergencia inválido. Solo números y mínimo 7 dígitos.");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // -------------------------
            // ACTUALIZAR CLIENTE
            // -------------------------
            Client.Id = Id;
            var clientRepo = CreateClientRepository();
            await clientRepo.UpdateAsync(Client);

            TempData["Message"] = $"Cliente '{Client.Name}' actualizado correctamente.";
            return RedirectToPage("/Users/User");
        }
    }
}
