using GYMPT.Domain.Entities;       
using GYMPT.Domain.Ports;          
using GYMPT.Domain.Rules;          
using GYMPT.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
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

        public async Task<IActionResult> OnPostAsync()
        {
            // -------------------------
            // VALIDACIONES DE USUARIO (por si alguien manipula el form)
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
            // VALIDACIONES DE DOMINIO CLIENT
            // -------------------------
            if (!ClientRules.FechaNacimientoValida(Client.DateBirth, 18))
                ModelState.AddModelError("Client.DateBirth", "El cliente debe tener al menos 18 años y la fecha no puede ser futura.");

            if (!ClientRules.NivelFitnessValido(Client.FitnessLevel))
                ModelState.AddModelError("Client.FitnessLevel", "Nivel de fitness inválido.");

            if (!ClientRules.PesoInicialValido(Client.InitialWeightKg))
                ModelState.AddModelError("Client.InitialWeightKg", "Peso inicial inválido.");

            if (!ClientRules.PesoActualValido(Client.InitialWeightKg, Client.CurrentWeightKg))
                ModelState.AddModelError("Client.CurrentWeightKg", "Peso actual no puede ser menor que el inicial.");

            if (!ClientRules.TelefonoEmergenciaValido(Client.EmergencyContactPhone))
                ModelState.AddModelError("Client.EmergencyContactPhone", "Teléfono inválido (mínimo 7 dígitos).");

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
