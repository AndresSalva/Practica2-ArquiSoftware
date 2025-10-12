using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace GYMPT.Pages.Users
{
    public class UserModel : PageModel
    {
        private readonly IUserService _userService;
        public IEnumerable<User> UserList { get; private set; } = new List<User>();
        public UserModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            UserList = await _userService.GetAllUsers();
        }
    }
}