using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Factories;
using GYMPT.Models;
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
        // Lista de usuarios que se mostrará en la vista
        // -------------------------
        public IEnumerable<User> UserList { get; private set; } = Enumerable.Empty<User>();

        // -------------------------
        // Parámetro para ordenar la tabla (desde query string)
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

            UserList = SortOrder?.ToLower() switch
            {
                "id" => users.OrderBy(u => u.Id),
                "name" => users.OrderBy(u => u.Name)
                               .ThenBy(u => u.FirstLastname)
                               .ThenBy(u => u.SecondLastname),
                "role" => users.OrderBy(u => u.Role),
                _ => users.OrderBy(u => u.Id)
            };
        }
    }
}
