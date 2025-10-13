using GYMPT.Domain.Entities;              // Entidades del dominio (User)
using GYMPT.Domain.Ports;                 // Interfaz IRepository<T>
using GYMPT.Domain.Rules;
using GYMPT.Infrastructure.Factories;     // Factoría del repositorio
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages.Users
{
    public class UserModel : PageModel
    {
        // -------------------------
        // Repositorio de Users
        // -------------------------
        private IRepository<User> CreateUserRepository()
        {
            var factory = new UserRepositoryCreator();
            return factory.CreateRepository();
        }

        // -------------------------
        // Lista de usuarios para mostrar en la vista
        // -------------------------
        public IEnumerable<User> UserList { get; private set; } = Enumerable.Empty<User>();

        // -------------------------
        // Parámetro de orden (query string)
        // -------------------------
        [BindProperty(SupportsGet = true)]
        public string? SortOrder { get; set; }

        // -------------------------
        // Carga de datos
        // -------------------------
        public async Task OnGetAsync()
        {
            var repo = CreateUserRepository();
            var users = await repo.GetAllAsync();

            // Lista total
            var allUsers = new List<User>();

            foreach (var user in users)
            {
                bool datosValidos =
                    UserRules.NombreCompletoValido(user.Name) &&
                    UserRules.NombreCompletoValido(user.FirstLastname) &&
                    (string.IsNullOrWhiteSpace(user.SecondLastname) || UserRules.NombreCompletoValido(user.SecondLastname)) &&
                    UserRules.CiValido(user.Ci) &&
                    UserRules.FechaNacimientoValida(user.DateBirth) &&
                    UserRules.RoleValido(user.Role);

                if (!datosValidos)
                    Console.WriteLine($"⚠️ Usuario con datos inválidos detectado: {user.Id} - {user.Name}");

                allUsers.Add(user);
            }

            // Ordenamiento
            UserList = SortOrder?.ToLower() switch
            {
                "id" => allUsers.OrderBy(u => u.Id),
                "name" => allUsers.OrderBy(u => u.Name)
                                   .ThenBy(u => u.FirstLastname)
                                   .ThenBy(u => u.SecondLastname),
                "role" => allUsers.OrderBy(u => u.Role),
                _ => allUsers.OrderBy(u => u.Id)
            };
        }

    }
}
