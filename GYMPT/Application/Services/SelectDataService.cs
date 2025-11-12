using GYMPT.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceDiscipline.Application.Interfaces;
using ServiceMembership.Application.Interfaces;
using ServiceUser.Application.Interfaces;
using ServiceClient.Application.Interfaces;

namespace GYMPT.Application.Services
{
    public class SelectDataService : ISelectDataService
    {
        // Este IUserService ahora apunta correctamente a ServiceClient.Application.Interfaces.IUserService
        private readonly IUserService _userService;
        // Este IMembershipService apunta correctamente a GYMPT.Application.Interfaces.IMembershipService
        private readonly IMembershipService _membershipService;
        private readonly IDisciplineService _disciplineService;
        private readonly IClientService _clientsService;


        public SelectDataService(IUserService userService, IMembershipService membershipService, IDisciplineService disciplineService, IClientService clientService)
        {
            _userService = userService;
            _membershipService = membershipService;
            _disciplineService = disciplineService;
            _clientsService = clientService;
        }

        public async Task<SelectList> GetUserOptionsAsync()
        {
            var users = await _clientsService.GetAllClients();
            if (users == null || !users.Any())
            {
                return new SelectList(Enumerable.Empty<SelectListItem>());
            }

            var userOptions = users
                .Select(u => new
                {
                    u.Id,
                    FullName = $"{u.Name ?? ""} {u.FirstLastname ?? ""}".Trim()
                })
                .ToList(); 

            return new SelectList(userOptions, "Id", "FullName");
        }

        public async Task<SelectList> GetMembershipOptionsAsync()
        {
            var membershipResult = await _membershipService.GetAllMemberships();
            if (membershipResult.IsFailure || membershipResult.Value is null)
            {
                throw new InvalidOperationException(membershipResult.Error ?? "No se pudo obtener la lista de membresías.");
            }

            return new SelectList(membershipResult.Value, "Id", "Name");
        }

        public async Task<SelectList> GetDisciplineOptionsAsync()
        {
            var disciplines = await _disciplineService.GetAllDisciplines();
            return new SelectList(disciplines, "Id", "Name");
        }

        public async Task<SelectList> GetInstructorOptionsAsync()
        {
            var users = await _userService.GetAllUsers();

            var instructors = users
                .Where(u => u.Role == "Instructor")
                .Select(u => new
                {
                    Id = (long)u.Id,
                    FullName = $"{u.Name} {u.FirstLastname}"
                });
            return new SelectList(instructors, "Id", "FullName");
        }
    }
}
