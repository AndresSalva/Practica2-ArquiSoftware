using GYMPT.Application.DTO;
using GYMPT.Application.Interfaces;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;

namespace GYMPT.Application.Facades
{
    public class PersonFacade
    {
        private readonly IUserService _userService;    // Admin / Instructor / otros
        private readonly IClientService _clientService; // Clientes

        public PersonFacade(IUserService userService, IClientService clientService)
        {
            _userService = userService;
            _clientService = clientService;
        }

        /// <summary>
        /// Obtiene todos los registros de personas (clientes y usuarios), opcionalmente filtrando por rol.
        /// </summary>
        /// <param name="roleFilter">Si se indica, solo devuelve personas con este rol ("Client", "Instructor", "Admin", etc.)</param>
        public async Task<List<PersonDto>> GetAllPersonsAsync()
        {
            // Obtener solo usuarios con rol (Instructor/Admin/otro)
            var users = await _userService.GetAllUsers(); // ya filtrados en SQL
                                                          // Obtener clientes
            var clients = await _clientService.GetAllClients();

            var result = new List<PersonDto>();

            // Mapear clientes
            result.AddRange(clients.Select(c => new PersonDto
            {
                Id = c.Id,
                Name = c.Name ?? "",
                FirstLastname = c.FirstLastname ?? "",
                SecondLastname = c.SecondLastname ?? "",
                Ci = c.Ci ?? "",
                DateBirth = c.DateBirth,
                Role = "Cliente"
            }));

           // Definir los roles que consideras "usuarios" (no clientes)
            var validUserRoles = new[] { "Instructor", "Admin"};

            result.AddRange(users
                .Where(u => !string.IsNullOrWhiteSpace(u.Role) && validUserRoles.Contains(u.Role))
                .Select(u => new PersonDto
                {
                    Id = u.Id, // id_user del JOIN
                    Name = u.Name ?? "",
                    FirstLastname = u.FirstLastname ?? "",
                    SecondLastname = u.SecondLastname ?? "",
                    Ci = u.Ci ?? "",
                    DateBirth = u.DateBirth,
                    Role = u.Role
                }));

            // Ordenar por nombre completo opcional
            return result.OrderBy(p => p.Name).ThenBy(p => p.FirstLastname).ThenBy(p => p.SecondLastname).ToList();
        }

        /// <summary>
        /// Elimina un cliente por su Id.
        /// </summary>
        public async Task<bool> DeleteClientAsync(int clientId)
        {
            return await _clientService.DeleteClient(clientId);
        }

        /// <summary>
        /// Elimina un usuario (Instructor/Admin) por su Id.
        /// </summary>
        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _userService.DeleteUser(userId);
        }
    }
}
