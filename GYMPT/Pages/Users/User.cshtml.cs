using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Factories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    public class UserModel : PageModel
    {
        private IRepository<User> CreateUserRepository()
        {
            var factory = new UserRepositoryCreator();
            return factory.CreateRepository();
        }

        public IEnumerable<User> UserList { get; private set; } = Enumerable.Empty<User>();

        public UserModel()
        {
        }

        public async Task OnGetAsync()
        {
            var repo = CreateUserRepository();
            UserList = await repo.GetAllAsync();
        }
    }
}
