using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceClient.Application.Interfaces;
using ServiceMembership.Application.Interfaces;
using ServiceUser.Application.Interfaces;

namespace GYMPT.Application.Facades
{
    public class SelectDataFacade : ISelectDataFacade
    {
        private readonly IUserService _userService;
        private readonly IClientService _clientService;
        private readonly IMembershipService _membershipService;

        public SelectDataFacade(
            IUserService userService,
            IClientService clientService,
            IMembershipService membershipService)
        {
            _userService = userService;
            _clientService = clientService;
            _membershipService = membershipService;
        }

        // 🔹 Listar instructores (user.role = 'Instructor')
        public async Task<SelectList> GetInstructorOptionsAsync()
        {
            var users = await _userService.GetAllUsers();
            var instructors = users
                .Where(u => u.Role != null && u.Role.Equals("Instructor", StringComparison.OrdinalIgnoreCase))
                .Select(u => new
                {
                    u.Id, // en la tabla 'user' el PK es id_person → corresponde con 'person.id'
                    FullName = $"{u.Name} {u.FirstLastname}"
                });

            return new SelectList(instructors, "Id", "FullName");
        }

        public async Task<SelectList> GetClientOptionsAsync()
        {
            var clients = await _clientService.GetAllAsync();
            var clientOptions = clients.Select(c => new
            {
                c.Id, // 'person.id'
                FullName = $"{c.Name} {c.FirstLastname}"
            });

            return new SelectList(clientOptions, "Id", "FullName");
        }

        // 🔹 Listar membresías
        public async Task<SelectList> GetMembershipOptionsAsync()
        {
            var memberships = await _membershipService.GetAllMemberships();
            var options = memberships.Value.Select(m => new
            {
                m.Id,
                m.Name
            });

            return new SelectList(options, "Id", "Name");
        }
    }
}
