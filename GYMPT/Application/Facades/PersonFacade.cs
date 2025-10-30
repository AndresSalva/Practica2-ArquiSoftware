using GYMPT.Application.DTO;
using ServiceClient.Application.Interfaces;
using ServiceUser.Application.Interfaces;

namespace GYMPT.Application.Facades
{
    public class PersonFacade
    {
        private readonly IUserService _userService;
        private readonly IClientService _clientService;

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
            var users = await _userService.GetAllUsers();
            var clients = await _clientService.GetAllAsync();

            var result = new List<PersonDto>();

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

            var validUserRoles = new[] { "Instructor", "Admin"};

            result.AddRange(users
                .Where(u => !string.IsNullOrWhiteSpace(u.Role) && validUserRoles.Contains(u.Role))
                .Select(u => new PersonDto
                {
                    Id = u.Id,
                    Name = u.Name ?? "",
                    FirstLastname = u.FirstLastname ?? "",
                    SecondLastname = u.SecondLastname ?? "",
                    Ci = u.Ci ?? "",
                    DateBirth = u.DateBirth,
                    Role = u.Role
                }));

            return result.OrderBy(p => p.Name).ThenBy(p => p.FirstLastname).ThenBy(p => p.SecondLastname).ToList();
        }

        /// <summary>
        /// Elimina un cliente por su Id.
        /// </summary>
        public async Task<bool> DeleteClientAsync(int clientId)
        {
            return await _clientService.DeleteByIdAsync(clientId);
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
