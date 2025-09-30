using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages
{
    public class UsersModel : PageModel
    {
        private readonly IRepository<UserData> _repo;

        public IEnumerable<UserData> UserList { get; private set; } = Enumerable.Empty<UserData>();

        public UsersModel(IRepository<UserData> repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            UserList = await _repo.GetAllAsync();
        }
    }
}
