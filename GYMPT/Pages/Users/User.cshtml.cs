using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Domain.Rules;
using GYMPT.Infrastructure.Factories;
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
        private IRepository<User> CreateUserRepository()
        {
            var factory = new UserRepositoryCreator();
            return factory.CreateRepository();
        }

        public class UserView
        {
            public User User { get; set; } = new User();
            public bool TieneDatosInvalidos { get; set; } = false; // Solo para visual
        }

        public IEnumerable<UserView> UserList { get; private set; } = Enumerable.Empty<UserView>();

        [BindProperty(SupportsGet = true)]
        public string? SortOrder { get; set; }

        public async Task OnGetAsync()
        {
            var repo = CreateUserRepository();
            var users = await repo.GetAllAsync();

            var allUsers = new List<UserView>();

            foreach (var user in users)
            {
                var nombreValido = UserRules.NombreCompletoValido(user.Name);
                var primerApellidoValido = UserRules.NombreCompletoValido(user.FirstLastname);
                var segundoApellidoValido = string.IsNullOrWhiteSpace(user.SecondLastname) || UserRules.NombreCompletoValido(user.SecondLastname).IsSuccess;
                var ciValido = UserRules.CiValido(user.Ci);
                var fechaNacimientoValida = UserRules.FechaNacimientoValida(user.DateBirth);
                var rolValido = UserRules.RoleValido(user.Role);

                bool tieneError = nombreValido.IsFailure || primerApellidoValido.IsFailure || !segundoApellidoValido ||
                                  ciValido.IsFailure || fechaNacimientoValida.IsFailure || rolValido.IsFailure;

                allUsers.Add(new UserView
                {
                    User = user,
                    TieneDatosInvalidos = tieneError
                });
            }

            // Ordenamiento
            UserList = SortOrder?.ToLower() switch
            {
                "id" => allUsers.OrderBy(u => u.User.Id),
                "name" => allUsers.OrderBy(u => u.User.Name)
                                 .ThenBy(u => u.User.FirstLastname)
                                 .ThenBy(u => u.User.SecondLastname),
                "role" => allUsers.OrderBy(u => u.User.Role),
                _ => allUsers.OrderBy(u => u.User.Id)
            };
        }
    }

}
