using GYMPT.Application.DTO;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
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

        public async Task<List<PersonDto>> GetAllPersonsAsync()
        {
            var users = await _userService.GetAllUsers();
            var clients = await _clientService.GetAllClients();

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

        public async Task<bool> DeleteClientAsync(int clientId)
        {
            var result = await _clientService.DeleteClient(clientId);
            if (result.IsSuccess)
            {
                return true;
            }
            return false;
        }


        public async Task<bool> DeleteUserAsync(int userId)
        {
            var result = await _userService.DeleteUser(userId);
            if (result.IsSuccess)
            {
                return true;
            }
            return false;
        }
    }
}
