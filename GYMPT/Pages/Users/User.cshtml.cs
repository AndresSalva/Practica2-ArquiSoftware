using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    public class UserModel : PageModel
    {
        private readonly UserRepository _repo;

        public IEnumerable<User> UserList { get; private set; } = Enumerable.Empty<User>();

        public UserModel(UserRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            UserList = await _repo.GetAllAsync();
        }
    }
}
