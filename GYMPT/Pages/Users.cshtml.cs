using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages
{
    public class Users : PageModel
    {
        private readonly IRepository<User> _repo;

        public IEnumerable<User> UserList { get; private set; } = Enumerable.Empty<User>();

        public Users(IRepository<User> repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            UserList = await _repo.GetAllAsync();
        }
    }
}
