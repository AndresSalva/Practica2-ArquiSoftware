using GYMPT.Data;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages
{
    public class UsersModel : PageModel
    {
        private readonly UserRepository _repo;

        public IEnumerable<User> UserList { get; private set; } = Enumerable.Empty<User>();

        public UsersModel(UserRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            UserList = await _repo.GetAllAsync();
        }
    }
}
