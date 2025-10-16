using GYMPT.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMPT.Application.Services
{
    public class SelectDataService : ISelectDataService
    {
        private readonly IUserService _userService;
        private readonly IMembershipService _membershipService;

        public SelectDataService(IUserService userService, IMembershipService membershipService)
        {
            _userService = userService;
            _membershipService = membershipService;
        }

        public async Task<SelectList> GetUserOptionsAsync()
        {
            var users = await _userService.GetAllUsers();
            var userOptions = users
                .Where(u => u.Role == "Client")
                .Select(u => new {
                u.Id,
                FullName = $"{u.Name} {u.FirstLastname}"
            });
            return new SelectList(userOptions, "Id", "FullName");
        }

        public async Task<SelectList> GetMembershipOptionsAsync()
        {
            var memberships = await _membershipService.GetAllMemberships();
            return new SelectList(memberships, "Id", "Name");
        }

        public async Task<SelectList> GetInstructorOptionsAsync()
        {
            var users = await _userService.GetAllUsers();
            var instructors = users
                .Where(u => u.Role == "Instructor")
                .Select(u => new {
                    Id = (long)u.Id,
                    FullName = $"{u.Name} {u.FirstLastname}"
                });
            return new SelectList(instructors, "Id", "FullName");
        }
    }
}