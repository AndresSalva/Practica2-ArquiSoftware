using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;
using GYMPT.Validations;
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
            // --- Validaciones ---
            if (!ValidacionesUsuario.EsNivelFitnessValido(Client.FitnessLevel))
                ModelState.AddModelError("Client.FitnessLevel", "Nivel de fitness inválido");

            if (Client.InitialWeightKg > 9999)
                ModelState.AddModelError("Client.InitialWeightKg", "Peso inicial inválido");

            if (Client.CurrentWeightKg > 9999)
                ModelState.AddModelError("Client.CurrentWeightKg", "Peso actual inválido");

            if (!ValidacionesUsuario.EsTelefonoValido(Client.EmergencyContactPhone))
                ModelState.AddModelError("Client.EmergencyContactPhone", "Teléfono inválido");

            if (!ModelState.IsValid)
                return Page();

            // --- Asegurar datos del User ---
            Client.Role = "Client";
            Client.CreatedAt = DateTime.UtcNow;
            Client.LastModification = DateTime.UtcNow;
            Client.IsActive = true;

            // Si faltan datos de Name, etc., los tomamos del formulario previo
            if (string.IsNullOrWhiteSpace(Client.Name))
                Client.Name = Name;
            if (string.IsNullOrWhiteSpace(Client.FirstLastname))
                Client.FirstLastname = FirstLastname;
            if (string.IsNullOrWhiteSpace(Client.SecondLastname))
                Client.SecondLastname = SecondLastname;
            if (string.IsNullOrWhiteSpace(Client.Ci))
                Client.Ci = Ci;
            if (Client.DateBirth == default)
                Client.DateBirth = DateBirth;

            // --- Guardar en DB ---
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
