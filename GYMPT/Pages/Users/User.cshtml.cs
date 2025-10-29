// Ruta: GYMPT/Pages/Users/UserModel.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

// --- CORRECCIÓN: Se necesitan las interfaces y entidades de ambos ---
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;

using GYMPT.Infrastructure.Services; // Para UrlTokenSingleton

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class UserModel : PageModel
    {
        // --- CORRECCIÓN: Inyectamos ambos servicios ---
        private readonly IUserService _userService;
        private readonly IClientService _clientService;
        private readonly UrlTokenSingleton _urlTokenSingleton;

        // Esta lista contendrá a TODOS: Clientes, Instructores y Admins.
        public IEnumerable<User> UserList { get; set; } = new List<User>();
        public Dictionary<int, string> UserTokens { get; set; } = new Dictionary<int, string>();

        public UserModel(IUserService userService, IClientService clientService, UrlTokenSingleton urlTokenSingleton)
        {
            _userService = userService;
            _clientService = clientService; // Se inyecta el servicio de clientes
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task OnGetAsync()
        {
            // --- ¡ESTA ES LA LÓGICA DE UNIFICACIÓN! ---

            // 1. Obtenemos la lista de usuarios del sistema (Instructores, Admins).
            var systemUsers = await _userService.GetAllAsync();

            // 2. Obtenemos la lista de clientes.
            var clients = await _clientService.GetAllAsync();

            // 3. Combinamos ambas listas en una sola.
            //    Esto es posible porque la clase Client hereda de User.
            var combinedList = new List<User>();
            combinedList.AddRange(systemUsers);
            combinedList.AddRange(clients);

            // 4. Asignamos la lista combinada y ordenada a la propiedad que usa la vista.
            UserList = combinedList.OrderBy(u => u.FirstLastname).ThenBy(u => u.Name);

            // 5. Generamos los tokens para todos en la lista unificada.
            UserTokens = UserList.ToDictionary(u => u.Id, u => _urlTokenSingleton.GenerateToken(u.Id.ToString()));
        }

        // Tu lógica de eliminación ya es correcta para los usuarios del sistema.
        // Si necesitas eliminar clientes, tendrías que añadir lógica aquí para llamar
        // a _clientService.DeleteByIdAsync(id) si el rol es "Client".
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            // (Este código se mantiene igual, pero podría mejorarse para manejar la eliminación de clientes también)
            try
            {
                // Primero intentamos eliminarlo como si fuera un User (Instructor/Admin)
                var success = await _userService.DeleteByIdAsync(id);

                // Si no se encontró (porque podría ser un Cliente), intentamos con el clientService
                if (!success)
                {
                    success = await _clientService.DeleteByIdAsync(id);
                }

                if (success)
                {
                    TempData["SuccessMessage"] = $"El registro con ID {id} fue eliminado correctamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar el registro. Es posible que ya haya sido eliminado.";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al intentar eliminar el registro.";
            }

            return RedirectToPage();
        }
    }
}