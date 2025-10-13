using GYMPT.Domain.Entities;       
using GYMPT.Domain.Ports;          
using GYMPT.Domain.Rules;
using GYMPT.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using GYMPT.Domain.Shared; 
using System;
using System.Threading.Tasks;

namespace GYMPT.Pages.Clients
{
    public class CreateModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FirstLastname { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SecondLastname { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Ci { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime DateBirth { get; set; }

        [BindProperty]
        public Client Client { get; set; } = new Client();

        private IRepository<Client> CreateClientRepository() => new ClientRepository();

        public void OnGet()
        {
            // Prellenar datos básicos del User
            Client.Name = Name;
            Client.FirstLastname = FirstLastname;
            Client.SecondLastname = SecondLastname;
            Client.Ci = Ci;
            Client.DateBirth = DateBirth;
            Client.Role = "Client";
        }

        // -------------------------
        // MÉTODO AUXILIAR PARA RESULT
        // -------------------------
        private void AddModelErrorIfFail(Result result, string key)
        {
            if (result.IsFailure)
                ModelState.AddModelError(key, result.Error);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // -------------------------
            // VALIDACIONES DE USUARIO
            // -------------------------
            AddModelErrorIfFail(UserRules.NombreCompletoValido(Client.Name), "Client.Name");
            AddModelErrorIfFail(UserRules.NombreCompletoValido(Client.FirstLastname), "Client.FirstLastname");
            AddModelErrorIfFail(UserRules.NombreCompletoValido(Client.SecondLastname), "Client.SecondLastname");
            AddModelErrorIfFail(UserRules.CiValido(Client.Ci), "Client.Ci");
            AddModelErrorIfFail(UserRules.FechaNacimientoValida(Client.DateBirth), "Client.DateBirth");

            // -------------------------
            // VALIDACIONES DE DOMINIO CLIENT
            // -------------------------
            AddModelErrorIfFail(ClientRules.FechaNacimientoValida(Client.DateBirth, 18), "Client.DateBirth");
            AddModelErrorIfFail(ClientRules.NivelFitnessValido(Client.FitnessLevel), "Client.FitnessLevel");
            AddModelErrorIfFail(ClientRules.PesoInicialValido(Client.InitialWeightKg), "Client.InitialWeightKg");
            AddModelErrorIfFail(ClientRules.PesoActualValido(Client.InitialWeightKg, Client.CurrentWeightKg), "Client.CurrentWeightKg");
            AddModelErrorIfFail(ClientRules.TelefonoEmergenciaValido(Client.EmergencyContactPhone), "Client.EmergencyContactPhone");

            if (!ModelState.IsValid)
                return Page();

            // -------------------------
            // PREPARAR DATOS PARA GUARDAR
            // -------------------------
            Client.Role = "Client";
            Client.CreatedAt = DateTime.UtcNow;
            Client.LastModification = DateTime.UtcNow;
            Client.IsActive = true;

            // Completar datos básicos si vienen vacíos
            Client.Name = string.IsNullOrWhiteSpace(Client.Name) ? Name : Client.Name;
            Client.FirstLastname = string.IsNullOrWhiteSpace(Client.FirstLastname) ? FirstLastname : Client.FirstLastname;
            Client.SecondLastname = string.IsNullOrWhiteSpace(Client.SecondLastname) ? SecondLastname : Client.SecondLastname;
            Client.Ci = string.IsNullOrWhiteSpace(Client.Ci) ? Ci : Client.Ci;
            Client.DateBirth = Client.DateBirth == default ? DateBirth : Client.DateBirth;

            // -------------------------
            // GUARDAR EN DB
            // -------------------------
            var clientRepo = CreateClientRepository();
            try
            {
                await clientRepo.CreateAsync(Client);
            }
            catch (PostgresException ex) when (ex.SqlState == "22003")
            {
                ModelState.AddModelError("", "Uno de los valores numéricos excede el límite permitido en la base de datos.");
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar cliente: {ex.Message}");
                return Page();
            }

            return RedirectToPage("/Users/User");
        }
    }
}
