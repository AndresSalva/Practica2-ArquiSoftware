using System.Threading.Tasks;
using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages
{
    public class DetailsUsersModel : PageModel
    {
        private readonly IRepository<DetailsUser> _repo;
        public IEnumerable<DetailsUser> DetailsUserList { get; private set; } = Enumerable.Empty<DetailsUser>();

        public DetailsUsersModel(IRepository<DetailsUser> repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            DetailsUserList = await _repo.GetAllAsync();
        }
    }
}
