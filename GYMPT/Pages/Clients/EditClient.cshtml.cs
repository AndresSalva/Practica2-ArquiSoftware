using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Factories;
using GYMPT.Domain.Rules;
using GYMPT.Domain.Shared;
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

        private void AddModelErrorIfFail(Result result, string key)
        {
            if (result.IsFailure)
                ModelState.AddModelError(key, result.Error);
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
            AddModelErrorIfFail(UserRules.NombreCompletoValido(Client.Name), "Client.Name");
            AddModelErrorIfFail(UserRules.NombreCompletoValido(Client.FirstLastname), "Client.FirstLastname");
            AddModelErrorIfFail(UserRules.NombreCompletoValido(Client.SecondLastname), "Client.SecondLastname");
            AddModelErrorIfFail(UserRules.CiValido(Client.Ci), "Client.Ci");
            AddModelErrorIfFail(UserRules.FechaNacimientoValida(Client.DateBirth), "Client.DateBirth");

            // -------------------------
            // VALIDACIONES DE DOMINIO
            // -------------------------
            AddModelErrorIfFail(ClientRules.FechaNacimientoValida(Client.DateBirth, 18), "Client.DateBirth");
            AddModelErrorIfFail(ClientRules.NivelFitnessValido(Client.FitnessLevel), "Client.FitnessLevel");
            AddModelErrorIfFail(ClientRules.PesoInicialValido(Client.InitialWeightKg), "Client.InitialWeightKg");
            AddModelErrorIfFail(ClientRules.PesoActualValido(Client.InitialWeightKg, Client.CurrentWeightKg), "Client.CurrentWeightKg");
            AddModelErrorIfFail(ClientRules.TelefonoEmergenciaValido(Client.EmergencyContactPhone), "Client.EmergencyContactPhone");

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
