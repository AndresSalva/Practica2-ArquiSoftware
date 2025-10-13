using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Factories;
using GYMPT.Domain.Rules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Clients
{
    public class EditClientModel : PageModel
    {
        [BindProperty]
        public Client Client { get; set; } = new Client();

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
                return RedirectToPage("/Users/User");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // -------------------------
            // VALIDACIONES DE INPUT
            // -------------------------
            if (!UserRules.NombreCompletoValido(Client.Name))
                ModelState.AddModelError("Client.Name", "Nombre inválido. Solo letras y mínimo 2 caracteres.");

            if (!UserRules.NombreCompletoValido(Client.FirstLastname))
                ModelState.AddModelError("Client.FirstLastname", "Apellido Paterno inválido.");

            if (!UserRules.NombreCompletoValido(Client.SecondLastname))
                ModelState.AddModelError("Client.SecondLastname", "Apellido Materno inválido.");

            if (!UserRules.CiValido(Client.Ci))
                ModelState.AddModelError("Client.Ci", "CI inválido. Solo números y letras.");

            // -------------------------
            // VALIDACIONES DE DOMINIO
            // -------------------------
            if (!ClientRules.FechaNacimientoValida(Client.DateBirth, 18))
                ModelState.AddModelError("Client.DateBirth", "El cliente debe tener al menos 18 años y la fecha no puede ser futura.");

            if (!ClientRules.NivelFitnessValido(Client.FitnessLevel))
                ModelState.AddModelError("Client.FitnessLevel", "Nivel de fitness inválido.");

            if (!ClientRules.PesoInicialValido(Client.InitialWeightKg))
                ModelState.AddModelError("Client.InitialWeightKg", "Peso inicial inválido.");

            if (!ClientRules.PesoActualValido(Client.InitialWeightKg, Client.CurrentWeightKg))
                ModelState.AddModelError("Client.CurrentWeightKg", "Peso actual no puede ser menor al inicial.");

            if (!ClientRules.TelefonoEmergenciaValido(Client.EmergencyContactPhone))
                ModelState.AddModelError("Client.EmergencyContactPhone", "Teléfono inválido.");

            if (!ModelState.IsValid)
                return Page();

            // -------------------------
            // ACTUALIZAR CLIENTE
            // -------------------------
            var clientRepo = CreateClientRepository();
            await clientRepo.UpdateAsync(Client);

            TempData["Message"] = $"Cliente '{Client.Name}' actualizado correctamente.";
            return RedirectToPage("/Users/User");
        }
    }
}
