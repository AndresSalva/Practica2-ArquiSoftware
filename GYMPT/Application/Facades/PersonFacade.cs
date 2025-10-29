using GYMPT.Application.DTO;
using GYMPT.Application.Interfaces;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;

namespace GYMPT.Application.Facades
{
    public class PersonFacade
    {
        private readonly IUserService _userService;   // para Instructor/Admin
        private readonly IClientService _clientService; // para Cliente

        public PersonFacade(IUserService userService, IClientService clientService)
        {
            _userService = userService;
            _clientService = clientService;
        }

        public async Task<List<PersonDto>> GetAllPersonsAsync()
        {
            var users = await _userService.GetAllUsers(); // devuelve User
            var clients = await _clientService.GetAllClients(); // devuelve Client

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
                Role = "Client"
            }));

            // Mapear usuarios
            result.AddRange(users.Select(u => new PersonDto
            {
                Id = u.Id,
                Name = u.Name ?? "",
                FirstLastname = u.FirstLastname ?? "",
                SecondLastname = u.SecondLastname ?? "",
                Ci = u.Ci ?? "",
                DateBirth = u.DateBirth,
                Role = u.Role ?? "Instructor" // ahora sí funciona porque u es User
            }));

            return result;
        }

        public async Task<bool> DeleteClientAsync(int clientId)
        {
            return await _clientService.DeleteClient(clientId);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            // Aquí podrías crear un método DeleteInstructorAsync en IUserService
            // para manejar solo usuarios con rol Instructor/Admin
            return await _userService.DeleteUser(userId);
        }
    }
}
